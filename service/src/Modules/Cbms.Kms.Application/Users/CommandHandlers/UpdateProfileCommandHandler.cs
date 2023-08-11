using Cbms.Application.Users.Dto;
using Cbms.Application.Users.Commands;
using Cbms.Application.Users.Query;
using Cbms.Authentication;
using Cbms.Authorization.Users;
using Cbms.Authorization.Users.Actions;
using Cbms.Domain.Repositories;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Cbms.Kms.Domain.Users;
using Cbms.Domain.Entities;
using System.Linq;
using Cbms.Kms.Domain;
using MediatR;

namespace Cbms.Application.Users.CommandHandlers
{
    public class UpdateProfileCommandHandler : CommandHandlerBase, IRequestHandler<UpdateProfileCommand,UserDto>
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<UserAssignment, int> _userAssignmentRepository;
        public UpdateProfileCommandHandler(
            IRequestSupplement supplement, 
            IRepository<User, int> userRepository, 
            IRepository<UserAssignment, int> userAssignmentRepository) : base(supplement)
        {
            _userRepository = userRepository;
            _userAssignmentRepository = userAssignmentRepository;
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
        }

        public async Task<UserDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            User user = await _userRepository.GetAsync(Session.UserId.Value);

            await user.ApplyActionAsync(new UpsertUserAction(
                user.UserName,
                entityDto.Name ?? "",
                string.IsNullOrEmpty(entityDto.Password) ? "" : PasswordManager.HashPassword(entityDto.Password),
                entityDto.EmailAddress,
                entityDto.PhoneNumber,
                entityDto.Birthday,
                user.ExpireDate,
                user.IsActive));

            string phoneNumber = (entityDto.PhoneNumber ?? "").Trim();

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var exists = _userRepository.GetAll().FirstOrDefault(p => p.UserName != user.UserName && p.PhoneNumber == phoneNumber);
                if (exists != null)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("User.ExistsPhoneNumber", phoneNumber).Build();
                }
            }

            await _userRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await Mediator.Send(new GetUserExt(user.Id));
        }
    }
}