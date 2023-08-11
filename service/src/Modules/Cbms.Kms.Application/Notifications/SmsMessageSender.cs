using Cbms.Dependency;
using Cbms.Kms.Application.Notifications.Dto;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Notifications;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Notifications
{
    public class SmsMessageSender : ISmsMessageSender, ITransientDependency
    {
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IAppSettingManager _appSettingManager;

        public SmsMessageSender(IConfiguration configuration, IAppLogger appLogger, IAppSettingManager appSettingManager)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _appSettingManager = appSettingManager;
        }

        public async Task ScheduleAsync(string phone, string content)
        {
            await _appLogger.LogInfoAsync("SCHEDULE_SEND_SMS", new { Phone = phone, Content = content });
            var jobId = BackgroundJob.Schedule<SmsMessageJob>(c => c.RunAsync(phone, content), TimeSpan.FromSeconds(15));
        }

        public async Task<bool> SendAsync(string phone, string content)
        {
            if (await _appSettingManager.IsEnableAsync("SMS_ENABLE"))
            {
                await _appLogger.LogInfoAsync("SEND_SMS", new { Phone = phone, Content = content, Message = "Start" });
                try
                {
                    var info = new SmsInfoDto()
                    {
                        u = _configuration["Sms:Account"],
                        pwd = _configuration["Sms:Password"],
                        from = _configuration["Sms:ServiceId"],
                        phone = phone,
                        sms = content
                    };

                    using (var client = new WebClient())
                    {
                        var url = _configuration["Sms:Url"] + string.Format("?u={0}&pwd={1}&from={2}&phone={3}&sms={4}&bid=1&type=8&json=1", info.u, info.pwd, info.from, info.phone, content);
                        var response = client.DownloadString(url);
                        var result = JsonConvert.DeserializeObject<SmsResultDto>(response);

                        if (result.error != "0")
                        {
                            await _appLogger.LogErrorAsync("SEND_SMS", new { Phone = phone, Content = content, Error = result });
                        }
                        else
                        {
                            await _appLogger.LogInfoAsync("SEND_SMS", new { Phone = phone, Content = content, Message = result });
                        }

                        return result.error == "0";
                    }
                }
                catch (Exception ex)
                {
                    await _appLogger.LogErrorAsync("SEND_SMS", new { Phone = phone, Content = content, Error = ex });
                    return false;
                }
            }
            return true;
        }
    }
}