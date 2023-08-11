using Cbms.Application.Authentication.Users.Dto;
using Cbms.Application.Users.Dto;
using Cbms.Application.Users.Query;
using Cbms.Authorization.Users;
using Cbms.Domain.Repositories;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserDto = Cbms.Application.Users.Dto.UserDto;

namespace Cbms.Application.Users.QueryHandlers
{
    public class GetUserExtHandler : QueryHandlerBase, IRequestHandler<GetUserExt, UserDto>
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepository<User, int> _repository;

        public GetUserExtHandler(IRequestSupplement supplement, IRepository<User, int> repository, AppDbContext dbContext) : base(supplement)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<UserDto> Handle(GetUserExt request, CancellationToken cancellationToken)
        {
            var entityDto = Mapper.Map<UserDto>(await _repository.GetAsync(request.Id));

            entityDto.Roles = await (from ur in _dbContext.UserRoles
                                     join role in _dbContext.Roles on ur.RoleId equals role.Id
                                     where ur.UserId == request.Id
                                     select new UserRoleDto()
                                     {
                                         CreationTime = ur.CreationTime,
                                         CreatorUserId = ur.CreatorUserId,
                                         Description = role.Description,
                                         DisplayName = role.DisplayName,
                                         Id = ur.Id,
                                         LastModificationTime = ur.LastModificationTime,
                                         LastModifierUserId = ur.LastModifierUserId,
                                         RoleId = ur.RoleId,
                                         RoleName = role.RoleName
                                     }).ToListAsync();

            entityDto.Assignments = await (from ur in _dbContext.UserAssignments
                                           join org in _dbContext.SalesOrgs on ur.SalesOrgId equals org.Id
                                           where ur.UserId == request.Id
                                           select new UserAssignmentDto()
                                           {
                                               CreationTime = ur.CreationTime,
                                               CreatorUserId = ur.CreatorUserId,
                                               SalesOrgName = org.Name,
                                               Id = ur.Id,
                                               SalesOrgId = org.Id,
                                               UserId = ur.UserId,
                                               LastModificationTime = ur.LastModificationTime,
                                               LastModifierUserId = ur.LastModifierUserId,
                                           }).ToListAsync();

            entityDto.RoleName = entityDto.Roles.FirstOrDefault().RoleName;

            return entityDto;
        }
    }
}