using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmPrices.Commands;
using Cbms.Kms.Domain.PosmPrices;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmPrices.CommandHandlers
{
    public class PosmPriceHeaderDeleteCommandHandler : DeleteEntityCommandHandler<PosmPriceHeaderDeleteCommand, PosmPriceHeader>
    {
        private readonly IRepository<PosmPriceHeader, int> _posmPriceRepository;

        public PosmPriceHeaderDeleteCommandHandler(IRequestSupplement supplement, IRepository<PosmPriceHeader, int> posmPriceRepository) : base(supplement)
        {
            _posmPriceRepository = posmPriceRepository;
        }

        public async override Task<Unit> Handle(PosmPriceHeaderDeleteCommand request, CancellationToken cancellationToken)
        {
            return await base.Handle(request, cancellationToken);
        }
    }
}