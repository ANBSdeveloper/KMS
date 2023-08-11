using Cbms.Domain.Repositories;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandlers
{
    public class TicketGetHandler : QueryHandlerBase, IRequestHandler<TicketGet, TicketDto>
    {
        private readonly IRepository<Ticket, int> _repository;

        public TicketGetHandler(IRequestSupplement supplement, IRepository<Ticket, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
        }

        public async Task<TicketDto> Handle(TicketGet request, CancellationToken cancellationToken)
        {
            return Mapper.Map<TicketDto>(await _repository.GetAsync(request.Id));
        }
    }
}