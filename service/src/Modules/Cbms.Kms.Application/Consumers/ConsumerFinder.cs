using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Consumers.Dto;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.Consumers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Consumers
{
    public class ConsumerFinder : IConsumerFinder, ITransientDependency
    {
        public readonly IRepository<Consumer, int> _consumerRepository;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly ILogger _logger;

        public ConsumerFinder(ILogger logger, IAppLogger appLogger, IRepository<Consumer, int> consumerRepository, IConfiguration configuration)
        {
            _consumerRepository = consumerRepository;
            _configuration = configuration;
            _appLogger = appLogger;
            _logger = logger;
        }

        public async Task<ConsumerInfo> FindByPhoneAsync(string phone)
        {
            var consumer = await _consumerRepository.FirstOrDefaultAsync(p => p.Phone == phone);
            if (consumer != null)
            {
                return new ConsumerInfo()
                {
                    Name = consumer.Name,
                    Phone = consumer.Phone
                };
            }
            return null;
        }

        public async Task<ConsumerInfo> FindByPhoneInSalesForce(string phone)
        {
            try
            {
                await _appLogger.LogInfoAsync("SALESFORCE_FIND_PHONE", new { Phone = phone });

                string apiUrl = _configuration["SalesForce:ApiUrl"];
                string authUrl = _configuration["SalesForce:AuthUrl"];
                string clientId = _configuration["SalesForce:ClientId"];
                string clientSecret = _configuration["SalesForce:ClientSecret"];
                string username = _configuration["SalesForce:Username"];
                string password = _configuration["SalesForce:Password"];
                using (var authClient = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        {"grant_type", "password" },
                        {"client_id", clientId},
                        {"client_secret" , clientSecret },
                        {"username", username },
                        {"password", password }
                    };

                    #region Api có https thì thêm vào

                    var contentLogin = new FormUrlEncodedContent(values);

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                           | SecurityProtocolType.Tls11
                           | SecurityProtocolType.Tls12
                           | SecurityProtocolType.Tls13;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                    #endregion Api có https thì thêm vào

                    // Post Async
                    var response = await authClient.PostAsync(authUrl, contentLogin);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<SalesForceLoginResultDto>(responseString);

                    if (result.access_token != null)
                    {
                        var client = new RestClient($@"{apiUrl}/query?q=SELECT+Phone,Id,Name,Shipping_Province__c,Shipping_Ward__c,Shipping_District__c,Shipping_Address__c,Province__c,Province__pc,RecordTypeId+FROM+Account+WHERE+Phone+='{phone}'");
                        client.UseNewtonsoftJson();
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("Authorization", $"Bearer {result.access_token}");
                        var consumerResult = await client.GetAsync<SalesForceFindResultDto>(request);
                        if (consumerResult.Records.Count > 0)
                        {
                            return new ConsumerInfo()
                            {
                                Name = consumerResult.Records[0].Name,
                                Phone = consumerResult.Records[0].Phone,
                            };
                        }
                        else
                        {
                            await _appLogger.LogErrorAsync("SALESFORCE_FIND_PHONE", new { Phone = phone, Message = "Not found" });
                        }
                    }
                    else
                    {
                        await _appLogger.LogErrorAsync("SALESFORCE_FIND_PHONE", new { Message = "Cant get token" });
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("SALESFORCE_FIND_PHONE", ex);
                _logger.Error(ex.ToString());

                throw ex;
            }
        }
    }
}