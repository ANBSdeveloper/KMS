using Cbms.Domain.Repositories;
using Cbms.Kms.Application.RewardPackages.Commands;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.RewardPackages.CommandHandlers
{
    public class RewardPackageDeleteCommandHandler : DeleteEntityCommandHandler<RewardPackageDeleteCommand, RewardPackage>
    {
        private readonly IRepository<RewardPackage, int> _rewardPackageRepository;

        public RewardPackageDeleteCommandHandler(IRequestSupplement supplement, IRepository<RewardPackage, int> rewardPackageRepository) : base(supplement)
        {
            _rewardPackageRepository = rewardPackageRepository;
        }

        public async override Task<Unit> Handle(RewardPackageDeleteCommand request, CancellationToken cancellationToken)
        {
            return await base.Handle(request, cancellationToken);
        }
    }
}