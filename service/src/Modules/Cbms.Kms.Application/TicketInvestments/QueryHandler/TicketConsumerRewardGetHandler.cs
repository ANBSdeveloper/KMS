using Cbms.Domain.Entities;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.QueryHandlers
{
    public class TicketConsumerRewardGetHandler : QueryHandlerBase, IRequestHandler<TicketConsumerRewardGet, TicketConsumerRewardDto>
    {
        private readonly AppDbContext _dbContext;

        public TicketConsumerRewardGetHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _dbContext = dbContext;
        }

        public async Task<TicketConsumerRewardDto> Handle(TicketConsumerRewardGet request, CancellationToken cancellationToken)
        {
            var entity = await (from p in _dbContext.TicketConsumerRewards
                                join r in _dbContext.RewardItems on p.RewardItemId equals r.Id
                                where p.Id == request.Id
                                select new TicketConsumerRewardDto()
                                {
                                    CreationTime = p.CreationTime,
                                    CreatorUserId = p.CreatorUserId,
                                    Id = p.Id,
                                    LastModificationTime = p.LastModificationTime,
                                    LastModifierUserId = p.LastModifierUserId,
                                    Photo1 = p.Photo1,
                                    Photo2 = p.Photo2,
                                    Photo3 = p.Photo3,
                                    Photo4 = p.Photo4,
                                    Photo5 = p.Photo5,
                                    Quantity = p.Quantity,
                                    RewardItemId = p.RewardItemId,
                                    RewardQuantity = p.RewardQuantity,
                                    RewardItemCode = r.Code,
                                    RewardItemName = r.Name
                                }).FirstOrDefaultAsync();
      
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TicketInvestment), request.Id);
            }

            var entityDto = Mapper.Map<TicketConsumerRewardDto>(entity);

            entityDto.Details = await (from p in _dbContext.TicketConsumerRewardDetails
                                       join t in _dbContext.Tickets on p.TicketId equals t.Id
                                       where p.TicketConsumerRewardId == entity.Id
                                       select new TicketConsumerRewardDetailDto()
                                       {
                                           Id = p.Id,
                                           TicketId = p.TicketId,
                                           TicketCode = t.Code,
                                           ConsumerName = t.ConsumerName,
                                           ConsumerPhone = t.ConsumerPhone,
                                           Note = p.Note,
                                           CreationTime = p.CreationTime,
                                           CreatorUserId = p.CreatorUserId,
                                           LastModificationTime = p.LastModificationTime,
                                           LastModifierUserId = p.LastModifierUserId
                                       }).ToListAsync();

            return entityDto;
        }
    }
}