using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.TicketInvestments.Commands;
using Cbms.Kms.Application.TicketInvestments.Dto;
using Cbms.Kms.Application.TicketInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using Cbms.Mediator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.TicketInvestments.CommandHandlers
{
    public class TicketInvestmentRegisterCommandHandler : RequestHandlerBase, IRequestHandler<TicketInvestmentRegisterCommand, TicketInvestmentDto>
    {
        private readonly IRepository<TicketInvestment, int> _ticketInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public TicketInvestmentRegisterCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<TicketInvestment, int> ticketInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _ticketInvestmentRepository = ticketInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<TicketInvestmentDto> Handle(TicketInvestmentRegisterCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            var ticketInvestment = new TicketInvestment();

            await using (await _distributedLockManager.AcquireAsync($"budget"))
            {
                await using (await _distributedLockManager.AcquireAsync($"ticket_investment_customer_"+ requestData.CustomerId))
                {
                    await ticketInvestment.ApplyActionAsync(new TicketInvestmentRegisterAction(
                    IocResolver,
                    LocalizationSource,
                    Session.UserId.Value,
                    requestData.CustomerId,
                    requestData.StockQuantity,
                    requestData.RewardPackageId,
                    requestData.PointsForTicket,
                    requestData.BuyBeginDate,
                    requestData.BuyEndDate,
                    requestData.IssueTicketBeginDate,
                    requestData.OperationDate,
                    requestData.RegisterNote,
                    requestData.SurveyPhoto1,
                    requestData.SurveyPhoto2,
                    requestData.SurveyPhoto3,
                    requestData.SurveyPhoto4,
                    requestData.SurveyPhoto5,
                    requestData.SalesCommitments.Select(p => new TicketInvestmentRegisterSalesCommitment(
                        p.Year,
                        p.Month,
                        p.Amount
                    )).ToList(),
                    requestData.Materials.Select(p => new TicketInvestmentRegisterMaterial(
                        p.MaterialId,
                        p.RegisterQuantity,
                        p.Note
                    )).ToList()
                ));

                    await _ticketInvestmentRepository.InsertAsync(ticketInvestment);

                    await _ticketInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);

                    return await Mediator.Send(new TicketInvestmentGet(ticketInvestment.Id));
                }
            }
        }
    }
}