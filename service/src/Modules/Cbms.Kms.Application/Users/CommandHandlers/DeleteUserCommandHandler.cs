using Cbms.Authorization.Users;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Users.Commands;
using Cbms.Kms.Domain.Users;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Users.CommandHandlers
{
    public class DeleteUserCommandHandler : DeleteEntityCommandHandler<DeleteUserCommand, User>
    {
        private readonly IRepository<UserAssignment, int> _assignmentRepository;
        public DeleteUserCommandHandler(IRequestSupplement supplement, IRepository<UserAssignment, int> assignmentRepository) : base(supplement)
        {
            _assignmentRepository = assignmentRepository;
        }

        public  async override Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var assignments = await _assignmentRepository.GetAll().Where(p => p.UserId == request.Id).ToListAsync();
            foreach (var item in assignments)
            {
                await _assignmentRepository.DeleteAsync(item);
            }
            return await base.Handle(request, cancellationToken);
        }
    }
}