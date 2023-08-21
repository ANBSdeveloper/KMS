using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.PosmInvestments.Commands;
using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using Cbms.Mediator;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.CommandHandlers
{
    public class PosmInvestmentRegisterCommandHandler : RequestHandlerBase, IRequestHandler<PosmInvestmentRegisterCommand, PosmInvestmentDto>
    {
        private readonly IRepository<PosmInvestment, int> _posmInvestmentRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public PosmInvestmentRegisterCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<PosmInvestment, int> PosmInvestmentRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _posmInvestmentRepository = PosmInvestmentRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<PosmInvestmentDto> Handle(PosmInvestmentRegisterCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;

            var posmInvestment = new PosmInvestment();

            await using (await _distributedLockManager.AcquireAsync($"budget"))
            {
                await using (await _distributedLockManager.AcquireAsync($"Posm_investment_customer_"+ requestData.CustomerId))
                {
                    await posmInvestment.ApplyActionAsync(new PosmInvestmentRegisterAction(
                        IocResolver,
                        LocalizationSource,
                        Session.UserId.Value,
                        requestData.CustomerId,
                        requestData.CustomerLocationId,
                        requestData.CurrentSalesAmount,
                        requestData.ShopPanelPhoto1,
                        requestData.ShopPanelPhoto2,
                        requestData.ShopPanelPhoto3,
                        requestData.ShopPanelPhoto4,
                        requestData.VisibilityPhoto1,
                        requestData.VisibilityPhoto2,
                        requestData.VisibilityPhoto3,
                        requestData.VisibilityPhoto4,
                        requestData.VisibilityCompetitorPhoto1,
                        requestData.VisibilityCompetitorPhoto2,
                        requestData.VisibilityCompetitorPhoto3,
                        requestData.VisibilityCompetitorPhoto4,
                        requestData.SetupContact1,
                        requestData.SetupContact2,
                        requestData.Note,
                        requestData.SalesCommitments.Select(p => new PosmInvestmentRegisterAction.SalesCommitment(
                            p.Year, 
                            p.Month, 
                            p.Amount)).ToList(),
                        requestData.Items.Select(p => new PosmInvestmentRegisterAction.PosmItem(
                            p.PanelShopName,
                            p.PanelShopPhone,
                            p.PanelShopAddress,
                            p.PanelOtherInfo,
                            p.PanelProductLine,
                            p.PosmCatalogId,
                            p.Width,
                            p.Height,
                            p.Depth,
                            p.SideWidth1,
                            p.SideWidth2,
                            p.Qty,
                            p.SetupPlanDate,
                            p.RequestType,
                            p.RequestReason,
                            p.Photo1,
                            p.Photo2,
                            p.Photo3,
                            p.Photo4)).ToList(),
						requestData.VTDCommitmentAmount,
						requestData.MilkIndustryAmount
						));

                    await _posmInvestmentRepository.InsertAsync(posmInvestment);

                    await _posmInvestmentRepository.UnitOfWork.CommitAsync(cancellationToken);

                    foreach (var item in posmInvestment.Items)
                    {
                        await item.LogHistoryAsync(IocResolver, LocalizationSource);
                    }
                    return await Mediator.Send(new PosmInvestmentGet(posmInvestment.Id));
                }
            }
        }
    }
}