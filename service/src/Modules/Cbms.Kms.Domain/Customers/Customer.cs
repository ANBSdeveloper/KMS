using Cbms.Authentication;
using Cbms.Authorization;
using Cbms.Authorization.Roles;
using Cbms.Authorization.Users;
using Cbms.Authorization.Users.Actions;
using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Extensions;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.Users;
using Cbms.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Customers
{
    public class Customer : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int? BranchId { get; private set; }
        public string ContactName { get; private set; }
        public string MobilePhone { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string ChannelCode { get; private set; }
        public string ChannelName { get; private set; }
        public string HouseNumber { get; private set; }
        public string Street { get; private set; }
        public string Address { get; private set; }
        public int? WardId { get; private set; }
        public int? DistrictId { get; private set; }
        public int? ProvinceId { get; private set; }
        public bool IsActive { get; private set; }
        public float Lat { get; private set; }
        public float Lng { get; private set; }
        public DateTime? Birthday { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public int? AreaId { get; private set; }
        public int? ZoneId { get; private set; }
        public bool IsKeyShop { get; private set; }
        public bool IsAllowKeyShopRegister { get; private set; }
        public KeyShopStatus KeyShopStatus { get; private set; }
        public string KeyShopAuthCode { get; private set; }
        public int? UserId { get; private set; }
        public decimal? Efficient { get; private set; }
        public string OtpCode { get; private set; }
        public DateTime? OtpTime { get; private set; }
        public int? KeyShopRegisterStaffId { get; private set; }
        public int? RsmStaffId { get; private set; }
        public int? AsmStaffId { get; private set; }
        public int? SalesSupervisorStaffId { get; private set; }
        public Customer()
        {
        }

        public static Customer Create()
        {
            return new Customer();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case CustomerRegisterAction registerAction:
                    await RegisterAsync(registerAction);
                    break;
                case CustomerUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                case CustomerRegisterKeyShopAction registerAction:
                    await RegisterKeyShopAsync(registerAction);
                    break;
                case CustomerApproveKeyShopAction approveAction:
                    await ApproveKeyShopAsync(approveAction);
                    break;
                case CustomerValidateActivationKeyShopAction validateAction:
                    await ValidateActivationKeyShopAsync(validateAction);
                    break;
                case CustomerActivateKeyShopAction activateAction:
                    await ActivateKeyShopAsync(activateAction);
                    break;
                case CustomerValidateRecoveryPasswordAction validateRecoveryAction:
                    await ValidateRecoveryPasswordAsync(validateRecoveryAction);
                    break;
                case CustomerResetPasswordAction resetPasswordAction:
                    await ResetPasswordAsync(resetPasswordAction);
                    break;
                case CustomerCheckOtpAction checkOtpAction:
                    await CheckOtpAsync(checkOtpAction);
                    break;
                case CustomerUpdateEfficientAction efficientAction:
                    await UpdateEfficientAsync(efficientAction);
                    break;
                case CustomerRefuseKeyShopAction refuseAction:
                    await RefuseKeyShopAsync(refuseAction);
                    break;
            }
        }


        public async Task UpdateEfficientAsync(CustomerUpdateEfficientAction action)
        {
            var appSettingManager = action.IocResolver.Resolve<IAppSettingManager>();
            var ticktInvestmentRepository = action.IocResolver.Resolve<IRepository<TicketInvestment, int>>();

            var efficentMonths = await appSettingManager.GetAsync("EFFICIENT_MONTHS");
            var months = string.IsNullOrEmpty(efficentMonths) ? 12 : Convert.ToInt32(efficentMonths);
            var toDate = Clock.Now.EndOfDay();
            var fromDate = toDate.AddMonths(-1 * months).Date;

            var remarks = ticktInvestmentRepository
                .GetAllIncluding(p => p.TicketAcceptance)
                .Where(p => p.CustomerId == Id &&
                    p.CreationTime >= fromDate && p.CreationTime <= toDate
                    && p.TicketAcceptance != null
                    && p.TicketAcceptance.RemarkOfCompany.HasValue)
                .Select(p => p.TicketAcceptance.RemarkOfCompany.Value)
                .ToList();

            var efficient = remarks.Count > 0 ?  Math.Round(remarks.Sum() / remarks.Count, 0) : 0;

            Efficient = efficient;

            await ticktInvestmentRepository.UnitOfWork.CommitAsync();
        }
        public async Task RegisterAsync(CustomerRegisterAction action)
        {
            var customerRepository = action.IocResolver.Resolve<IRepository<Customer, int>>();

            if (string.IsNullOrEmpty(action.UserName))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).FieldRequired("Username").Build();
            }

            if (action.UserName.Length < 6)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.UserNameLengthInvalid").Build();
            }
            if (action.UserName.Trim().Contains(" "))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.UserNameSpaceInvalid").Build();
            }
            
            if (string.IsNullOrEmpty(action.FullName))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).FieldRequired("Fullname").Build();
            }

            if (string.IsNullOrEmpty(action.Password))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).FieldRequired("Password").Build();
            }

            if (action.Password.Trim().Length < 6)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).FieldRequired("Customer.PasswordLengthInvalid").Build();
            }

            Regex regex = new Regex("^[0-9]{9,15}$");
            if (!regex.IsMatch(action.Phone ?? ""))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.MobilePhoneInvalid").Build();
            }

            var existsSamePhone = customerRepository.GetAll().FirstOrDefault(p => p.MobilePhone == action.Phone);
            if (existsSamePhone != null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("User.ExistsPhoneNumber", action.Phone).Build();
            }

            var sampleShop = customerRepository.GetAll().Where(p => p.IsKeyShop).FirstOrDefault();
            var userRepository = action.IocResolver.Resolve<IRepository<User, int>>();
            var roleRepository = action.IocResolver.Resolve<IRepository<Role, int>>();
            var user = new User();

            await user.ApplyActionAsync(new CreateUserAction(
                action.UserName, 
                action.FullName, 
                PasswordManager.HashPassword(action.Password), 
                null, 
                action.Phone, 
                null, 
                null,
                true)
            );

            var role = roleRepository.GetAll().Where(p => p.RoleName == KmsConsts.ShopRole).FirstOrDefault();
            var userRole = new UserRole();

            await userRole.ApplyActionAsync(new UpsertUserRoleAction(role.Id));
            await user.ApplyActionAsync(new CrudRoleToUserAction(
              new List<UserRole>() { userRole },
              new List<UserRole>()
            ));

            await userRepository.InsertAsync(user);

            Code = action.UserName;
            Name = action.FullName; 
            BranchId = sampleShop.BranchId;
            ContactName = action.FullName;
            MobilePhone = action.Phone;
            Phone = (sampleShop.Phone ?? "").Replace(".", "");
            Email = sampleShop.Email;
            ChannelCode = sampleShop.ChannelCode;
            ChannelName = sampleShop.ChannelName;
            HouseNumber = sampleShop.HouseNumber;
            Street = sampleShop.Street;
            Address = sampleShop.Address;
            WardId = sampleShop.WardId;
            DistrictId = sampleShop.DistrictId;
            ProvinceId = sampleShop.ProvinceId;
            IsActive = sampleShop.IsActive;
            Lat = sampleShop.Lat;
            Lng = sampleShop.Lng;
            Birthday = sampleShop.Birthday;
            UpdateDate = new DateTime(1999,1,1);
            AreaId = sampleShop.AreaId;
            ZoneId = sampleShop.ZoneId;
            RsmStaffId = sampleShop.RsmStaffId;
            AsmStaffId = sampleShop.AsmStaffId;
            SalesSupervisorStaffId = sampleShop.SalesSupervisorStaffId;
            UserId = user.Id;

            IsKeyShop = true;
            IsAllowKeyShopRegister = false;
            KeyShopStatus = KeyShopStatus.Approved;
        }

        public async Task UpsertAsync(CustomerUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            BranchId = action.BranchId;
            ContactName = action.ContactName;
            MobilePhone = action.MobilePhone;
            Phone = (action.Phone ?? "").Replace(".", "");
            Email = action.Email;
            ChannelCode = action.ChannelCode;
            ChannelName = action.ChannelName;
            HouseNumber = action.HouseNumber;
            Street = action.Street;
            Address = action.Address;
            WardId = action.WardId;
            DistrictId = action.DistrictId;
            ProvinceId = action.ProvinceId;
            IsActive = action.IsActive;
            Lat = action.Lat;
            Lng = action.Lng;
            Birthday = action.Birthday;
            UpdateDate = action.UpdateDate;
            AreaId = action.AreaId;
            ZoneId = action.ZoneId;
            RsmStaffId = action.RsmStaffId;
            AsmStaffId = action.AsmStaffId;
            SalesSupervisorStaffId = action.SalesSupervisorStaffId;
            if (action.IsNew)
            {
                IsKeyShop = false;
                IsAllowKeyShopRegister = true;
                KeyShopStatus = KeyShopStatus.Unregistered;
            }

        }
        public async Task RegisterKeyShopAsync(CustomerRegisterKeyShopAction action)
        {
            if (KeyShopStatus != KeyShopStatus.Unregistered && KeyShopStatus != KeyShopStatus.Refuse)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.RegisterInvalid", Code).Build();
            }
            IsKeyShop = false;
            IsAllowKeyShopRegister = false;
            KeyShopStatus = KeyShopStatus.Created;
            KeyShopRegisterStaffId = action.StaffId;
        }

        public async Task ApproveKeyShopAsync(CustomerApproveKeyShopAction action)
        {
            if (KeyShopStatus != KeyShopStatus.Created)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.ApproveInvalid", Code).Build();
            }
            IsKeyShop = false;
            IsAllowKeyShopRegister = false;
            KeyShopStatus = KeyShopStatus.Approved;
            KeyShopAuthCode = GetRandomCode(6);

            //TODO: Thêm insert thông báo
        }
        public async Task RefuseKeyShopAsync(CustomerRefuseKeyShopAction action)
        {
            if (KeyShopStatus != KeyShopStatus.Created)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.RefuseInvalid", Code).Build();
            }
            IsKeyShop = false;
            IsAllowKeyShopRegister = true;
            KeyShopStatus = KeyShopStatus.Refuse;
            KeyShopAuthCode = GetRandomCode(6);

            //TODO: Thêm insert thông báo
        }

        public async Task ValidateActivationKeyShopAsync(CustomerValidateActivationKeyShopAction action)
        {
            if (KeyShopStatus != KeyShopStatus.Approved)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.CantActive").Build();
            }

            if (KeyShopAuthCode != action.AuthCode)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.AuthCodeInvalid").Build();
            }

            if (action.Birthday == default(DateTime))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.BirthdayInvalid").Build();
            }

            Regex regex = new Regex("^[0-9]{9,15}$");
            if (!regex.IsMatch(action.MobilePhone ?? ""))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.MobilePhoneInvalid").Build();
            }

            var customerRepository = action.IocResolver.Resolve<IRepository<Customer, int>>();
            var otherCustomer = customerRepository.GetAll().Where(p => p.MobilePhone == action.MobilePhone && p.Code != Code && p.IsKeyShop).FirstOrDefault();

            if (otherCustomer != null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.KeyShopExistsWithPhone", action.MobilePhone).Build();
            }

            OtpTime = Clock.Now;
            MobilePhone = action.MobilePhone;
            ContactName = action.Name;
            Birthday = action.Birthday;

            var appSettingManager = action.IocResolver.Resolve<IAppSettingManager>();
            if (await appSettingManager.IsEnableAsync("SMS_ENABLE"))
            {
                OtpCode = (new Random()).Next(100000, 999999).ToString();
                string mesageTemplate = await action.IocResolver.Resolve<IAppSettingManager>().GetAsync("SHOP_REGISTER_OTP_SMS_TEMPLATE");
                string mesageOtp = string.IsNullOrEmpty(mesageTemplate) ? OtpCode : string.Format(mesageTemplate, OtpCode);
                var messageSender = action.IocResolver.Resolve<ISmsMessageSender>();
                var isSent = await messageSender.SendAsync(MobilePhone, mesageOtp);
                if (!isSent)
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.CantSendOtp").Build();
                }
            }
            else
            {
                OtpCode = "000000";
            }
        }

        public async Task ValidateRecoveryPasswordAsync(CustomerValidateRecoveryPasswordAction action)
        {
            if (!IsKeyShop)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.NotKeyShop").Build();
            }
    
            OtpTime = Clock.Now;

            var appSettingManager = action.IocResolver.Resolve<IAppSettingManager>();
            if (await appSettingManager.IsEnableAsync("SMS_ENABLE"))
            {
                OtpCode = (new Random()).Next(100000, 999999).ToString();
                string mesageTemplate = await action.IocResolver.Resolve<IAppSettingManager>().GetAsync("SHOP_RESET_PASSWORD_OTP_SMS_TEMPLATE");
                string mesageOtp = string.IsNullOrEmpty(mesageTemplate) ? OtpCode : string.Format(mesageTemplate, OtpCode);
                var messageSender = action.IocResolver.Resolve<ISmsMessageSender>();
                var isSent = await messageSender.SendAsync(MobilePhone, mesageOtp);
                if (!isSent)
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.CantSendOtp").Build();
                }
            }
            else
            {
                OtpCode = "000000";
            }
        }

        public async Task ResetPasswordAsync(CustomerResetPasswordAction action)
        {
            if (!IsKeyShop)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.NotKeyShop").Build();
            }

            if (OtpCode != action.OtpCode)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.OtpInvalid").Build();
            }

            if (Clock.Now.Subtract(OtpTime.Value).TotalMinutes > 5)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.OtpOverTime", "5").Build();
            }

            if (string.IsNullOrEmpty(action.NewPassword) || action.NewPassword.Length < 6)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.PaswordInvalid").Build();
            }

            OtpCode = string.Empty;
            OtpTime = null;

            var userManager = action.IocResolver.Resolve<IUserManager>();
            await userManager.ChangePassword(UserId.Value, action.NewPassword);
        }

        public async Task CheckOtpAsync(CustomerCheckOtpAction action)
        {
            if (!IsKeyShop)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.NotKeyShop").Build();
            }

            if (OtpCode != action.OtpCode)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.OtpInvalid").Build();
            }

            if (Clock.Now.Subtract(OtpTime.Value).TotalMinutes > 5)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.OtpOverTime", "5").Build();
            }
        }

        public async Task ActivateKeyShopAsync(CustomerActivateKeyShopAction action)
        {
            if (KeyShopStatus != KeyShopStatus.Approved)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.CantActive").Build();
            }

            if (OtpCode != action.OtpCode)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.OtpInvalid").Build();
            }

            if (Clock.Now.Subtract(OtpTime.Value).TotalMinutes > 5)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Customer.OtpOverTime", "5").Build();
            }

            OtpCode = "";
            OtpTime = null;
            IsKeyShop = true;
            IsActive = true;
            KeyShopStatus = KeyShopStatus.Registered;
            var userCreator = action.IocResolver.Resolve<IUserCreator>();
            var user = await userCreator.CreateAsync(KmsConsts.ShopRole, Code, action.Password.Trim(), Name, Email, MobilePhone, Birthday);
            UserId = user.Id;

        }

        private string GetRandomCode(int length)
        {        
            var allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
         
            var chars = new char[length];
            var rd = new Random();

            for (var i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}