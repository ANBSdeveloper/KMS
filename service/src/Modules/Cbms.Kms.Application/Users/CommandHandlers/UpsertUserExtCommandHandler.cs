using Cbms.Application.Users.Dto;
using Cbms.Application.Users.Commands;
using Cbms.Application.Users.Query;
using Cbms.Authentication;
using Cbms.Authorization.Users;
using Cbms.Authorization.Users.Actions;
using Cbms.Domain.Repositories;
using Cbms.Mediator;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cbms.Kms.Domain.Users;
using Cbms.Domain.Entities;
using Cbms.Kms.Domain.UserSalesOrgs.Actions;
using System.Linq;
using Cbms.Kms.Domain;

namespace Cbms.Application.Users.CommandHandlers
{
    public class UpsertUserExtCommandHandler : UpsertEntityCommandHandler<UpsertUserExtCommand, GetUserExt, UserDto>
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<UserAssignment, int> _userAssignmentRepository;
        public UpsertUserExtCommandHandler(
            IRequestSupplement supplement, 
            IRepository<User, int> userRepository, 
            IRepository<UserAssignment, int> userAssignmentRepository) : base(supplement)
        {
            _userRepository = userRepository;
            _userAssignmentRepository = userAssignmentRepository;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        protected override async Task<UserDto> HandleCommand(UpsertUserExtCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            User entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _userRepository.GetAsync(request.Data.Id);

                if (entity == null)
                {
                    throw new EntityNotFoundException(typeof(User), request.Data.Id);
                }
            }
            else
            {
                var diffUserWithSameUserName = await _userRepository.FirstOrDefaultAsync(p => p.UserName == entityDto.UserName );

                if (diffUserWithSameUserName != null)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("User.UserNameExists", entityDto.UserName).Build();
                }

                entity = new User();
                await _userRepository.InsertAsync(entity);
            }

            await entity.ApplyActionAsync(new UpsertUserAction(
                entityDto.UserName, 
                entityDto.Name, 
                string.IsNullOrEmpty(entityDto.Password) ? "" :  PasswordManager.HashPassword(entityDto.Password),
                entityDto.EmailAddress, 
                entityDto.PhoneNumber, 
                entityDto.Birthday, 
                entityDto.ExpireDate,
                entityDto.IsActive));

            await entity.ApplyActionAsync(new CrudRoleToUserAction(
               Mapper.Map<List<UserRole>>(entityDto.RoleChanges.UpsertedItems),
               Mapper.Map<List<UserRole>>(entityDto.RoleChanges.UpsertedItems)
            ));

            string phoneNumber = (entityDto.PhoneNumber ?? "").Trim();

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var exists = _userRepository.GetAll().FirstOrDefault(p => p.UserName != entityDto.UserName && p.PhoneNumber == phoneNumber);
                if (exists != null)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("User.ExistsPhoneNumber", phoneNumber).Build();
                }
            }
            foreach (var item in entityDto.AssignmentChanges.DeletedItems)
            {
                var assignment = await _userAssignmentRepository.FindAsync(item.Id);
                if (assignment != null)
                {
                    await _userAssignmentRepository.DeleteAsync(assignment);
                }
            }

            foreach (var item in entityDto.AssignmentChanges.UpsertedItems)
            {
               
                UserAssignment assignment = null;

                if (!item.Id.IsNew())
                {
                    assignment = _userAssignmentRepository.FirstOrDefault(p => p.Id == item.Id);
                    if (assignment == null)
                    {
                        throw new EntityNotFoundException(typeof(UserAssignment), item.Id);
                    }
                }
                else
                {
                    assignment = _userAssignmentRepository.FirstOrDefault(p => p.UserId == entity.Id && p.SalesOrgId == item.SalesOrgId);
                    if (assignment == null)
                    {
                        assignment = UserAssignment.Create();
                        await _userAssignmentRepository.InsertAsync(assignment);
                    }
                }

                await assignment.ApplyActionAsync(new UpsertUserAssignmentAction(
                    entity.Id,
                    item.SalesOrgId
                ));
                
            }

            await _userRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}