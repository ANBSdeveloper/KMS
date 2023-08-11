using Cbms.Authentication;
using Cbms.Authorization.Roles;
using Cbms.Authorization.Users;
using Cbms.Authorization.Users.Actions;
using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Staffs.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Staffs
{
    public class Staff : AuditedAggregateRoot
    {
        public Staff()
        {
        }

        public int? AreaId { get; private set; }
        public DateTime? Birthday { get; private set; }
        public string Code { get; private set; }
        public int CreditPoint { get; private set; }
        public string Email { get; private set; }
        public bool IsActive { get; private set; }
        public string MobilePhone { get; private set; }
        public string Name { get; private set; }
        public int SalesOrgId { get; private set; }
        public string StaffTypeCode { get;private set; }
        public string StaffTypeName { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public int? UserId { get; private set; }
        public int? ZoneId { get; private set; }
        public static Staff Create()
        {
            return new Staff();
        }

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case StaffRegisterAction registerAction:
                    await RegisterAsync(registerAction);
                    break;
                case StaffUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                case StaffUpdateCreditPointAction updateCreditPointAction:
                    await UpdateCreditPointAsync(updateCreditPointAction);
                    break;
            }
        }

        public async Task RegisterAsync(StaffRegisterAction action)
        {
            var customerRepository = action.IocResolver.Resolve<IRepository<Staff, int>>();

            if (string.IsNullOrEmpty(action.UserName))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).FieldRequired("Username").Build();
            }

            if (action.UserName.Length < 6)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Staff.UserNameLengthInvalid").Build();
            }
            if (action.UserName.Trim().Contains(" "))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Staff.UserNameSpaceInvalid").Build();
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
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).FieldRequired("Staff.PasswordLengthInvalid").Build();
            }

            Regex regex = new Regex("^[0-9]{9,15}$");
            if (!regex.IsMatch(action.Phone ?? ""))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Staff.MobilePhoneInvalid").Build();
            }

            var existsSamePhone = customerRepository.GetAll().FirstOrDefault(p => p.MobilePhone == action.Phone);
            if (existsSamePhone != null)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("User.ExistsPhoneNumber", action.Phone).Build();
            }

            var sampleStaff = customerRepository.GetAll().Where(p => p.IsActive).FirstOrDefault();
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

            var role = roleRepository.GetAll().Where(p => p.RoleName == KmsConsts.SalesSupervisorRole).FirstOrDefault();
            var userRole = new UserRole();

            await userRole.ApplyActionAsync(new UpsertUserRoleAction(role.Id));
            await user.ApplyActionAsync(new CrudRoleToUserAction(
              new List<UserRole>() { userRole },
              new List<UserRole>()
            ));

            await userRepository.InsertAsync(user);

            Code = action.UserName;
            Name = action.FullName;
            MobilePhone = action.Phone;
            Email = sampleStaff.Email;
            IsActive = sampleStaff.IsActive;
            Birthday = sampleStaff.Birthday;
            UpdateDate = new DateTime(1999, 1, 1);
            AreaId = sampleStaff.AreaId;
            ZoneId = sampleStaff.ZoneId;
            UserId = user.Id;
            CreditPoint = 0;
            SalesOrgId = sampleStaff.SalesOrgId;
            StaffTypeCode = sampleStaff.StaffTypeCode;
            StaffTypeName = sampleStaff.StaffTypeName;
        }

        private async Task UpdateCreditPointAsync(StaffUpdateCreditPointAction action)
        {
            CreditPoint = action.CreditPoints;
        }

        private async Task UpsertAsync(StaffUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            SalesOrgId = action.SalesOrgId;
            UpdateDate = action.UpdateDate;
            StaffTypeCode = action.StaffTypeCode;
            StaffTypeName = action.StaffTypeName;
            MobilePhone = action.MobilePhone;
            Birthday = action.Birthday;
            Email = action.Email;
            UserId = action.UserId;
            AreaId = action.AreaId;
            ZoneId = action.ZoneId;
            IsActive = action.IsActive;
        }
    }
}
