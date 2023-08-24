using Cbms.Domain.Entities;
using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Application.PosmInvestments.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Infrastructure;
using Cbms.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.PosmInvestments.QueryHandlers
{
    public class PosmInvestmentItemGetHandler : QueryHandlerBase, IRequestHandler<PosmInvestmentItemGet, PosmInvestmentItemDto>
    {
        private readonly AppDbContext _dbContext;

        public PosmInvestmentItemGetHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _dbContext = dbContext;
        }

        public async Task<PosmInvestmentItemDto> Handle(PosmInvestmentItemGet request, CancellationToken cancellationToken)
        {
            var entityDto = await (from p in _dbContext.PosmInvestmentItems
                                join i in _dbContext.PosmCatalogs on p.PosmCatalogId equals i.Id
                                join o in _dbContext.PosmItems on i.PosmItemId equals o.Id
                                join c in _dbContext.PosmClasses on o.PosmClassId equals c.Id
                                join d in _dbContext.PosmInvestments on p.PosmInvestmentId equals d.Id
                                where p.Id == request.Id
                                select new PosmInvestmentItemDto()
                                {
                                    TotalCost = p.TotalCost,
                                    ActualTotalCost = p.ActualTotalCost,
                                    CreationTime = p.CreationTime,
                                    CreatorUserId = p.CreatorUserId,
                                    Id = p.Id,
                                    LastModificationTime = p.LastModificationTime,
                                    LastModifierUserId = p.LastModifierUserId,
                                    Photo1 = p.Photo1,
                                    Photo2 = p.Photo2,
                                    Photo3 = p.Photo3,
                                    Photo4 = p.Photo4,
                                    PosmCatalogId = p.PosmCatalogId,
                                    AcceptanceDate = p.AcceptanceDate,
                                    AcceptanceNote = p.AcceptanceNote,
                                    AcceptancePhoto1 = p.AcceptancePhoto1,
                                    AcceptancePhoto2 = p.AcceptancePhoto2,
                                    AcceptancePhoto3 = p.AcceptancePhoto3,
                                    AcceptancePhoto4 = p.AcceptancePhoto4,
                                    ActualUnitPrice = p.ActualUnitPrice,
                                    Height = p.Height,
                                    OperationDate = p.OperationDate,
                                    OperationNote = p.OperationNote,
                                    OperationLink = p.OperationLink,
                                    OperationPhoto1 = p.OperationPhoto1,
                                    OperationPhoto2 = p.OperationPhoto2,
                                    OperationPhoto3 = p.OperationPhoto3,
                                    OperationPhoto4 = p.OperationPhoto4,
                                    PanelOtherInfo = p.PanelOtherInfo,
                                    PanelProductLine = p.PanelProductLine,
                                    PanelShopAddress = p.PanelShopAddress,
                                    PanelShopName = p.PanelShopName,
                                    PanelShopPhone = p.PanelShopPhone,
                                    PosmClassId = p.PosmClassId,
                                    PosmInvestmentId = p.PosmInvestmentId,
                                    PosmItemId = p.PosmItemId,
                                    PosmValue = p.PosmValue,
                                    PrepareDate = p.PrepareDate,
                                    PrepareNote = p.PrepareNote,
                                    Qty = p.Qty,
                                    RequestReason = p.RequestReason,
                                    RequestType = p.RequestType,
                                    SetupPlanDate = p.SetupPlanDate,
                                    SideWidth1 = p.SideWidth1,
                                    SideWidth2 = p.SideWidth2,
                                    Depth = p.Depth,
                                    Size = p.Size,
                                    Status = p.Status,
                                    UnitPrice = p.UnitPrice,
                                    UpdateCostReason = p.UpdateCostReason,
                                    VendorId = p.VendorId,
                                    Width = p.Width,
                                    PosmItemCode = o.Code,
                                    PosmItemName = o.Name,
                                    UnitType = o.UnitType,
                                    CalcType = o.CalcType,
                                    InclueInfo = c.IncludeInfo,
                                    RemarkOfCompany = p.RemarkOfCompany,
                                    RemarkOfSales = p.RemarkOfSales,
									DesignPhoto1 = d.DesignPhoto1,
									DesignPhoto2 = d.DesignPhoto2,
									DesignPhoto3 = d.DesignPhoto3,
									DesignPhoto4 = d.DesignPhoto4,
								}).FirstOrDefaultAsync();

            if (entityDto == null)
            {
                throw new EntityNotFoundException(typeof(PosmInvestmentItem), request.Id);
            }

          
            return entityDto;
        }
    }
}