using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Kms.Domain.Notifications;
using Cbms.Timing;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Consumers
{
    public class Consumer : AuditedAggregateRoot
    {
        public string Phone { get; private set; }
        public string Name { get; private set; }
        public string OtpCode { get; private set; }
        public DateTime? OtpTime { get; private set; }
        public Consumer()
        {

        }
        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case ConsumerCreateAction createAction:
                    await CreateAsync(createAction);
                    break;

                case ConsumerSendOtpAction sendAction:
                    await SendOtpAsync(sendAction);
                    break;

                case ConsumerValidateOtpAction validateAction:
                    await ValidateOtpAsync(validateAction);
                    break;
            }
        }

        public async Task CreateAsync(ConsumerCreateAction action)
        {
            Phone = action.Phone;
            Name = action.Name;
            OtpCode = "";
        }

        public async Task SendOtpAsync(ConsumerSendOtpAction action)
        {
            Regex regex = new Regex("^[0-9]{9,15}$");
            if (!regex.IsMatch(action.Phone ?? ""))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Consumer.PhoneInvalid").Build();
            }

            Phone = action.Phone;

            var appSettingManager = action.IocResolver.Resolve<IAppSettingManager>();
            if (await appSettingManager.IsEnableAsync("SMS_ENABLE"))
            {
                OtpCode = new Random().Next(100000, 999999).ToString();
            }
            else
            {
                OtpCode = "000000";
            }

           
            OtpTime = Clock.Now;

            var messageSender = action.IocResolver.Resolve<ISmsMessageSender>();

            var isSent = await messageSender.SendAsync(Phone, OtpCode);
            if (!isSent)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Consumer.CantSendOtp", action.Phone).Build();
            }
        }

        public async Task ValidateOtpAsync(ConsumerValidateOtpAction action)
        {
            if (OtpCode != action.OtpCode)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Consumer.OtpInvalid").Build();
            }

            if (Clock.Now.Subtract(OtpTime.Value).TotalMinutes > 5)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Consumer.OtpOverTime", "5").Build();
            }

            OtpCode = string.Empty;
            OtpTime = null;
        }
    }
}