using Cbms.Dependency;
using Cbms.Kms.Application.Integration.Dto;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.Integration;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Integration
{
    public class RewardAppManager: IRewardAppManager, ISingletonDependency
    {
       
        private IConfiguration _configuration;
        private IAppLogger _appLogger;
        public RewardAppManager(IConfiguration configuration, IAppLogger appLogger)
        {
            _configuration = configuration;
            _appLogger = appLogger;
        }
        public RestClient CreateClient(string url) 
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            return client;
        }
        private IRestRequest AddAuthentication(IRestRequest request)
        {
            request.AddHeader("x-app-id", _configuration["RewardApp:AppId"]);
            request.AddHeader("x-access-token", _configuration["RewardApp:AppToken"]);
            return request;
        }

        public async Task<object> FetchSpoon(string spoonCode)
        {
            try
            {
                string url = _configuration["RewardApp:Url"];
                var client = CreateClient(@$"{url}/api/ext/spoon?code={spoonCode}");
                IRestRequest request = new RestRequest(Method.GET);
                request = AddAuthentication(request);
                var response = client.Get<RewardAppResultDto<SpoonCodeResultDto>>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                await _appLogger.LogErrorAsync("CHECK_SPOON_CODE_RA", new { code = spoonCode, error = ex.ToString() });
                throw ex;
            }
        }

        public async Task<bool> SyncQrCode(string shopCode, DateTime scanDate, DateTime beginDate, DateTime endDate, List<string> qrCodes)
        {
            var data = new
            {
                qrCodes,
                scanDate = scanDate.ToUniversalTime(),
                beginDate = beginDate.ToUniversalTime(),
                endDate = endDate.ToUniversalTime(),
                shopCode
            };

            try
            {
                await _appLogger.LogInfoAsync("SYNC_QRCODE_REQUEST", new
                {
                    data
                });

                string url = _configuration["RewardApp:Url"];
                IRestRequest request = new RestRequest(Method.POST);
                request = request.AddJsonBody(data);
                request = AddAuthentication(request);
                var client = CreateClient($@"{url}/api/ext/lucky-draw");
                var response = client.Post<RewardAppResultDto<object>>(request);
                if (response.Data.Meta.Status == 1000)
                {
                    await _appLogger.LogInfoAsync("SYNC_QRCODE_COMPLETE", new
                    {
                        data,
                        result = response.Data
                    });
                    return true;
                }
                await _appLogger.LogErrorAsync("SYNC_QRCODE_ERROR", new {
                    data,
                    error = response.Data
                });

                return false;
            }
            catch(Exception ex)
            {
                await _appLogger.LogErrorAsync("SYNC_QRCODE_ERROR", new
                {
                    data,
                    error = ex.ToString()
                });

                return false;
            }
        }
    }
}
