using Cbms.Domain.Entities;
using Cbms.Kms.Application.TicketInvestments.Dto;
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
    public class TicketInvestmentGetHandler : QueryHandlerBase, IRequestHandler<TicketInvestmentGet, TicketInvestmentDto>
    {
        private readonly AppDbContext _dbContext;

        public TicketInvestmentGetHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _dbContext = dbContext;
        }

        public async Task<TicketInvestmentDto> Handle(TicketInvestmentGet request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.TicketInvestments
                .Where(p => p.Id == request.Id).FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TicketInvestment), request.Id);
            }

            var entityDto = Mapper.Map<TicketInvestmentDto>(entity);

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == entity.CustomerId);
            entityDto.CustomerName = customer.Name;
            entityDto.MobilePhone = customer.MobilePhone;
            entityDto.Address = customer.Address;
            entityDto.Efficient = customer.Efficient;

            entityDto.SalesCommitments = await (from p in _dbContext.TicketSalesCommitments
                                                where p.TicketInvestmentId == entity.Id
                                                select new TicketSalesCommitmentDto()
                                                {
                                                    Amount = p.Amount,
                                                    Id = p.Id,
                                                    Month = p.Month,
                                                    Year = p.Year,
                                                    CreationTime = p.CreationTime,
                                                    CreatorUserId = p.CreatorUserId,
                                                    LastModificationTime = p.LastModificationTime,
                                                    LastModifierUserId = p.LastModifierUserId,
                                                }).ToListAsync();

            entityDto.RewardItems = await (from p in _dbContext.TicketRewardItems
                                           join i in _dbContext.RewardItems on p.RewardItemId equals i.Id
                                           where p.TicketInvestmentId == entity.Id
                                           select new TicketRewardItemDto()
                                           {
                                               Amount = p.Amount,
                                               CreationTime = p.CreationTime,
                                               CreatorUserId = p.CreatorUserId,
                                               Id = p.Id,
                                               LastModificationTime = p.LastModificationTime,
                                               LastModifierUserId = p.LastModifierUserId,
                                               Price = p.Price,
                                               Quantity = p.Quantity,
                                               RewardItemId = p.RewardItemId,
                                               RewardItemName = i.Name,
                                               RewardItemCode = i.Code,
                                               DocumentLink = i.DocumentLink
                                           }).ToListAsync();

            entityDto.Progresses = await (from p in _dbContext.TicketUpdates
                                          join user in _dbContext.Users on p.UpdateUserId equals user.Id into userLeft
                                          from user in userLeft.DefaultIfEmpty()
                                          where p.TicketInvestmentId == entity.Id
                                          select new TicketProgressDto()
                                          {
                                              CreationTime = p.CreationTime,
                                              CreatorUserId = p.CreatorUserId,
                                              DocumentPhoto1 = p.DocumentPhoto1,
                                              DocumentPhoto2 = p.DocumentPhoto2,
                                              DocumentPhoto3 = p.DocumentPhoto3,
                                              DocumentPhoto4 = p.DocumentPhoto4,
                                              DocumentPhoto5 = p.DocumentPhoto5,
                                              Id = p.Id,
                                              LastModificationTime = p.LastModificationTime,
                                              LastModifierUserId = p.LastModifierUserId,
                                              Note = p.Note,
                                              UpdateTime = p.UpdateTime,
                                              UpdateUserId = p.UpdateUserId,
                                              UpdateUserName = user.Name
                                          }).OrderByDescending(p => p.CreationTime).ToListAsync();

            foreach (var item in entityDto.Progresses)
            {
                item.Materials = await (from p in _dbContext.TicketProgressMaterials
                                        join m in _dbContext.Materials on p.MaterialId equals m.Id
                                        join tm in _dbContext.TicketMaterials on new { TicketInvestmentId = entity.Id, p.MaterialId } equals new { tm.TicketInvestmentId, tm.MaterialId }
                                        where p.TicketProgressId == item.Id
                                        select new TicketProgressMaterialDto()
                                        {
                                            CreationTime = p.CreationTime,
                                            CreatorUserId = p.CreatorUserId,
                                            Id = p.Id,
                                            IsDesign = m.IsDesign,
                                            IsReceived = p.IsReceived,
                                            IsSentDesign = p.IsSentDesign,
                                            LastModificationTime = p.LastModificationTime,
                                            LastModifierUserId = p.LastModifierUserId,
                                            MaterialCode = m.Code,
                                            MaterialId = p.MaterialId,
                                            MaterialName = m.Name,
                                            Photo1 = p.Photo1,
                                            Photo2 = p.Photo2,
                                            Photo3 = p.Photo3,
                                            Photo4 = p.Photo4,
                                            Photo5 = p.Photo5,
                                            Price = tm.Price,
                                            RegisterQuantity = tm.RegisterQuantity,
                                            Amount = tm.Amount
                                        }).ToListAsync();

                item.RewardItems = await (from p in _dbContext.TicketProgressRewardItems
                                          join tr in _dbContext.TicketRewardItems on new { TicketInvestmentId = entity.Id, p.RewardItemId } equals new { tr.TicketInvestmentId, tr.RewardItemId }
                                          join r in _dbContext.RewardItems on tr.RewardItemId equals r.Id
                                          where p.TicketProgressId == item.Id
                                          select new TicketProgressRewardItemDto()
                                          {
                                              CreationTime = p.CreationTime,
                                              CreatorUserId = p.CreatorUserId,
                                              Id = p.Id,
                                              IsReceived = p.IsReceived,
                                              LastModificationTime = p.LastModificationTime,
                                              LastModifierUserId = p.LastModifierUserId,
                                              RewardItemName = r.Name,
                                              RewardItemId = tr.RewardItemId,
                                              DocumentLink = r.DocumentLink,
                                              RewardItemCode = r.Code,
                                              Price = tr.Price,
                                              Quantity = tr.Quantity
                                          }).ToListAsync();
            }

            entityDto.Materials = await (from p in _dbContext.TicketMaterials
                                         join m in _dbContext.Materials on p.MaterialId equals m.Id
                                         where p.TicketInvestmentId == entity.Id
                                         select new TicketMaterialDto()
                                         {
                                             Amount = p.Amount,
                                             CreationTime = p.CreationTime,
                                             CreatorUserId = p.CreatorUserId,
                                             Id = p.Id,
                                             IsDesign = m.IsDesign,
                                             LastModificationTime = p.LastModificationTime,
                                             LastModifierUserId = p.LastModifierUserId,
                                             MaterialId = p.MaterialId,
                                             MaterialCode = m.Code,
                                             MaterialName = m.Name,
                                             Price = p.Price,
                                             RegisterQuantity = p.RegisterQuantity,
                                             Note = p.Note ?? ""
                                         }).ToListAsync();

            entityDto.ConsumerRewards = await (from p in _dbContext.TicketConsumerRewards
                                               join tr in _dbContext.TicketRewardItems on new { TicketInvestmentId = entity.Id, p.RewardItemId } equals new { tr.TicketInvestmentId, tr.RewardItemId }
                                               join r in _dbContext.RewardItems on p.RewardItemId equals r.Id
                                               where p.TicketInvestmentId == entity.Id
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
                                                   RewardItemCode = r.Code,
                                                   RewardItemName = r.Name,
                                                   Quantity = p.Quantity,
                                                   RewardQuantity = p.RewardQuantity,
                                                   RewardItemId = p.RewardItemId
                                               }).ToListAsync();

            foreach (var item in entityDto.ConsumerRewards)
            {
                item.Details = await (from p in _dbContext.TicketConsumerRewardDetails
                                      join t in _dbContext.Tickets on p.TicketId equals t.Id
                                      where p.TicketConsumerRewardId == item.Id
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
            }

            entityDto.Acceptance = await (from p in _dbContext.TicketAcceptances
                                          join user in _dbContext.Users on p.UpdateUserId equals user.Id into userLeft
                                          from user in userLeft.DefaultIfEmpty()
                                          where p.TicketInvestmentId == entity.Id
                                          select new TicketAcceptanceDto()
                                          {
                                              AcceptanceDate = p.AcceptanceDate,
                                              CreationTime = p.CreationTime,
                                              CreatorUserId = p.CreatorUserId,
                                              Id = p.Id,
                                              LastModificationTime = p.LastModificationTime,
                                              LastModifierUserId = p.LastModifierUserId,
                                              Note = p.Note,
                                              Photo1 = p.Photo1,
                                              Photo2 = p.Photo2,
                                              Photo3 = p.Photo3,
                                              Photo4 = p.Photo4,
                                              Photo5 = p.Photo5,
                                              UpdateUserId = p.UpdateUserId,
                                              UpdateUserName = user.Name
                                          }).FirstOrDefaultAsync();

            entityDto.Operation = await (from p in _dbContext.TicketOperations
                                         join user in _dbContext.Users on p.UpdateUserId equals user.Id into userLeft
                                         from user in userLeft.DefaultIfEmpty()
                                         where p.TicketInvestmentId == entity.Id
                                         select new TicketOperationDto()
                                         {
                                             CreationTime = p.CreationTime,
                                             CreatorUserId = p.CreatorUserId,
                                             Id = p.Id,
                                             LastModificationTime = p.LastModificationTime,
                                             LastModifierUserId = p.LastModifierUserId,
                                             Note = p.Note,
                                             Photo1 = p.Photo1,
                                             Photo2 = p.Photo2,
                                             Photo3 = p.Photo3,
                                             Photo4 = p.Photo4,
                                             Photo5 = p.Photo5,
                                             StockQuantity = p.StockQuantity,
                                             UpdateUserId = p.UpdateUserId,
                                             UpdateUserName = user.Name,
                                             OperationDate = p.OperationDate,
                                         }).FirstOrDefaultAsync();

            entityDto.FinalSettlement = await (from p in _dbContext.TicketFinalSettlements
                                               join updateUser in _dbContext.Users on p.UpdateUserId equals updateUser.Id into updateUserLeft
                                               from updateUser in updateUserLeft.DefaultIfEmpty()
                                               join decideUser in _dbContext.Users on p.UpdateUserId equals decideUser.Id into userDecideLeft
                                               from decideUser in userDecideLeft.DefaultIfEmpty()
                                               where p.TicketInvestmentId == entity.Id
                                               select new TicketFinalSettlementDto()
                                               {
                                                   CreationTime = p.CreationTime,
                                                   CreatorUserId = p.CreatorUserId,
                                                   Date = p.Date,
                                                   DecideUserId = p.DecideUserId,
                                                   DecideUserName = decideUser.Name,
                                                   Id = p.Id,
                                                   LastModificationTime = p.LastModificationTime,
                                                   LastModifierUserId = p.LastModifierUserId,
                                                   Note = p.Note,
                                                   UpdateUserId = p.UpdateUserId,
                                                   UpdateUserName = updateUser.Name
                                               }).FirstOrDefaultAsync();
            return entityDto;
        }
    }
}