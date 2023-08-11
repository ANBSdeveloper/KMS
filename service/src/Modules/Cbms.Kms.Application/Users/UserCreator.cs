using AutoMapper;
using Cbms.Application.Authentication.Users.Dto;
using Cbms.Authentication;
using Cbms.Authorization.Roles;
using Cbms.Authorization.Users;
using Cbms.Authorization.Users.Actions;
using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Users;
using Cbms.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Users
{
    public class UserCreator : IUserCreator, ITransientDependency
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly ILocalizationManager _localizationManager;
        private readonly IMapper _mapper;
        public UserCreator(
            IRepository<User, int> userRepository, 
            IRepository<Role, int> roleRepository,
            IMapper mapper, 
            ILocalizationManager localizationManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _localizationManager = localizationManager;
        }

        public async Task<User> CreateAsync(string roleName, string userName, string password, string name, string email, string mobilePhone, DateTime? birthday)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(p => p.RoleName == roleName);
            if (role == null)
            {
                throw BusinessExceptionBuilder.Create(_localizationManager.GetDefaultSource()).MessageCode("Role.NotExists", roleName).Build();
            }
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(p => p.UserName == userName);
            if (user != null)
            {
                throw BusinessExceptionBuilder.Create(_localizationManager.GetDefaultSource()).MessageCode("User.UserNameExists", userName).Build();
            }
          
            if (string.IsNullOrEmpty(password) || password.Length <6)
            {
                throw BusinessExceptionBuilder.Create(_localizationManager.GetDefaultSource()).MessageCode("User.PasswordInvalidLength", "6").Build();
            }

            user = new User();

            await user.ApplyActionAsync(new UpsertUserAction(
                userName,
                name,
                PasswordManager.HashPassword(password),
                email,
                mobilePhone,
                birthday,
                null,
                true));

            await user.ApplyActionAsync(new CrudRoleToUserAction(
                _mapper.Map<List<UserRole>>(new List<UserRoleDto>() { new UserRoleDto() { RoleId = role.Id } }),
              new List<UserRole>()
             ));

            await _userRepository.InsertAsync(user);

            await _userRepository.UnitOfWork.CommitAsync();

            return user;
        }
    }
}