using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmInvestments.Actions;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Localization.Sources;
using Cbms.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Cbms.Kms.Domain.PosmInvestments
{
    public class PosmInvestmentItem : AuditedEntity
    {
        public int PosmClassId { get; private set; }
        public int PosmItemId { get; private set; }
        public string PanelShopName { get; private set; }
        public string PanelShopPhone { get; private set; }
        public string PanelShopAddress { get; private set; }
        public string PanelOtherInfo { get; private set; }
        public string PanelProductLine { get; private set; }
        public int PosmCatalogId { get; private set; }
        public decimal? Width { get; private set; }
        public decimal? Height { get; private set; }
        public decimal? Depth { get; private set; }
        public decimal? SideWidth1 { get; private set; }
        public decimal? SideWidth2 { get; private set; }
        public int Qty { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal? ActualUnitPrice { get; private set; }
        public decimal TotalCost { get; private set; }
        public decimal? ActualTotalCost { get; private set; }
        public decimal? Size { get; private set; }
        public decimal PosmValue { get; private set; }
        public DateTime SetupPlanDate { get; private set; }
        public PosmInvestmentItemStatus Status { get; private set; }
        public PosmRequestType RequestType { get; private set; }
        public string RequestReason { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
        public int PosmInvestmentId { get; private set; }
        public string PrepareNote { get; private set; }
        public DateTime? PrepareDate { get; private set; }
        public string UpdateCostReason { get; private set; }
        public string OperationPhoto1 { get; private set; }
        public string OperationPhoto2 { get; private set; }
        public string OperationPhoto3 { get; private set; }
        public string OperationPhoto4 { get; private set; }
        public string OperationLink { get; private set; }
        public string OperationNote { get; private set; }

        public DateTime? OperationDate { get; private set; }
        
        public string AcceptancePhoto1 { get; private set; }
        public string AcceptancePhoto2 { get; private set; }
        public string AcceptancePhoto3 { get; private set; }
        public string AcceptancePhoto4 { get; private set; }
        public DateTime? AcceptanceDate { get; private set; }
        public string AcceptanceNote { get; private set; }
        public int? VendorId { get; private set; }
        public decimal? RemarkOfSales { get; private set; }
        public decimal? RemarkOfCompany { get; private set; }
        public IReadOnlyCollection<PosmInvestmentItemHistory> Histories => _histories;
        public List<PosmInvestmentItemHistory> _histories = new List<PosmInvestmentItemHistory>();

        public override async Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmInvestmentItemImportHistoryAction importAction:
                    await ImportHistoryAsync(importAction);
                    break;

                case PosmInvestmentItemRegisterAction upsertAction:
                    await RegisterAsync(upsertAction);
                    break;

             
                case PosmInvestmentSupplyConfirmRequestAction procConfirmAction:
                    await SupplyConfirmAsync(procConfirmAction);
                    break;

                case PosmInvestmentSupplyDenyRequestAction procDenyAction:
                    await SupplyDenyAsync(procDenyAction);
                    break;

                case PosmInvestmentSupSuggestAction supSuggestAction:
                    await SupSuggestAsync(supSuggestAction);
                    break;

                case PosmInvestmentAsmConfirmSuggestAction asmConfirmSuggestAction:
                    await AsmConfirmSuggestAsync(asmConfirmSuggestAction);
                    break;

                case PosmInvestmentRsmConfirmSuggestAction rsmConfirmSuggestAction:
                    await RsmConfirmSuggestAsync(rsmConfirmSuggestAction);
                    break;

                case PosmInvestmentTradeConfirmSuggestAction tradeConfirmSuggestAction:
                    await TradeConfirmSuggestAsync(tradeConfirmSuggestAction);
                    break;
                case PosmInvestmentMarketingConfirmProduceAction designerConfirmProduceAction:
                    await MarketingConfirmProduceAsync(designerConfirmProduceAction);
                    break;
                case PosmInvestmentSupConfirmProduceAction supConfirmProduceAction:
                    await SupConfirmProduceAsync(supConfirmProduceAction);
                    break;
                case PosmInvestmentSupplyConfirmProduceAction procConfirmProduceAction:
                    await ProcConfirmProduceAsync(procConfirmProduceAction);
                    break;
                case PosmInvestmentSupAcceptAction supAcceptAction:
                    await SupAcceptAsync(supAcceptAction);
                    break;
                case PosmInvestmentAsmConfirmAcceptAction asmConfirmAcceptAction:
                    await AsmConfirmAcceptAsync(asmConfirmAcceptAction);
                    break;
                case PosmInvestmentTradeConfirmAcceptAction tradeConfirmAcceptAction:
                    await TradeConfirmAcceptAsync(tradeConfirmAcceptAction);
                    break;
                case PosmInvestmentItemClearAction clearAction:
                    await ClearAsync(clearAction);
                    break;
                case PosmInvestmentAsmApproveRequestAction asmApproveAction:
                    await AsmApproveRequestAsync(asmApproveAction);
                        break;
                case PosmInvestmentAsmDenyRequestAction asmDenyAction:
                    await AsmDenyRequestAsync(asmDenyAction);
                    break;
                case PosmInvestmentRsmApproveRequestAction rsmApproveAction:
                    await RsmApproveRequestAsync(rsmApproveAction);
                    break;
                case PosmInvestmentRsmDenyRequestAction rsmDenyAction:
                    await RsmDenyRequestAsync(rsmDenyAction);
                    break;
                case PosmInvestmentTradeApproveRequestAction tradeApproveAction:
                    await TradeApproveRequestAsync(tradeApproveAction);
                    break;
                case PosmInvestmentTradeDenyRequestAction tradeDenyAction:
                    await TradeDenyRequestAsync(tradeDenyAction);
                    break;
                case PosmInvestmentDirectorApproveRequestAction directorApproveAction:
                    await DirectorApproveAsync(directorApproveAction);
                    break;
                case PosmInvestmentDirectorDenyRequestAction directorDenyAction:
                    await DirectorDenyAsync(directorDenyAction);
                    break;
                case PosmInvestmentSalesRemarkAction salesRemarkAction:
                    await SalesRemarkAsync(salesRemarkAction);
                    break;
                case PosmInvestmentCompanyRemarkAction companyRemarkAction:
                    await CompanyRemarkAsync(companyRemarkAction);
                    break;
                default:
                    throw new ArgumentException();
            }
        }
        private async Task AsmApproveRequestAsync(PosmInvestmentAsmApproveRequestAction action)
        {
            Status = PosmInvestmentItemStatus.AsmApprovedRequest;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task AsmDenyRequestAsync(PosmInvestmentAsmDenyRequestAction action)
        {
            Status = PosmInvestmentItemStatus.AsmDeniedRequest;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task RsmApproveRequestAsync(PosmInvestmentRsmApproveRequestAction action)
        {
            Status = PosmInvestmentItemStatus.RsmApprovedRequest;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task RsmDenyRequestAsync(PosmInvestmentRsmDenyRequestAction action)
        {
            Status = PosmInvestmentItemStatus.RsmDeniedRequest;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task TradeApproveRequestAsync(PosmInvestmentTradeApproveRequestAction action)
        {
            Status = PosmInvestmentItemStatus.TradeApprovedRequest;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task TradeDenyRequestAsync(PosmInvestmentTradeDenyRequestAction action)
        {
            Status = PosmInvestmentItemStatus.TradeDeniedRequest;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task ClearAsync(PosmInvestmentItemClearAction action)
        {
            Photo1 = null;
            Photo2 = null;
            Photo3 = null;
            Photo4 = null;
            OperationPhoto1 = null;
            OperationPhoto2 = null;
            OperationPhoto3 = null;
            OperationPhoto4 = null;
            AcceptancePhoto1 = null;
            AcceptancePhoto2 = null;
            AcceptancePhoto3 = null;
            AcceptancePhoto4 = null;
            AcceptancePhoto1= null;
            AcceptancePhoto2= null;
            AcceptancePhoto3= null;
            AcceptancePhoto4= null;
        }

        private async Task DirectorApproveAsync(PosmInvestmentDirectorApproveRequestAction action)
        {
            Status = PosmInvestmentItemStatus.DirectorApprovedRequest;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task DirectorDenyAsync(PosmInvestmentDirectorDenyRequestAction action)
        {
            Status = PosmInvestmentItemStatus.DirectorDeniedRequest;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task SupSuggestAsync(PosmInvestmentSupSuggestAction action)
        {
            if (Status != PosmInvestmentItemStatus.InvalidOrder)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            Status = PosmInvestmentItemStatus.SupSuggestedUpdateCost;
            UpdateCostReason = action.Reason ?? string.Empty;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task AsmConfirmSuggestAsync(PosmInvestmentAsmConfirmSuggestAction action)
        {
            if (Status != PosmInvestmentItemStatus.SupSuggestedUpdateCost)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            Status = PosmInvestmentItemStatus.AsmConfirmedUpdateCost;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task RsmConfirmSuggestAsync(PosmInvestmentRsmConfirmSuggestAction action)
        {
            if (Status != PosmInvestmentItemStatus.AsmConfirmedUpdateCost)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            Status = PosmInvestmentItemStatus.RsmConfirmedUpdateCost;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task TradeConfirmSuggestAsync(PosmInvestmentTradeConfirmSuggestAction action)
        {
            if (Status != PosmInvestmentItemStatus.RsmConfirmedUpdateCost)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            Status = PosmInvestmentItemStatus.TradeConfirmedUpdateCost;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task MarketingConfirmProduceAsync(PosmInvestmentMarketingConfirmProduceAction action)
        {
            if (Status != PosmInvestmentItemStatus.ValidOrder)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            Status = PosmInvestmentItemStatus.ConfirmedProduce1;
            var imageResizer = action.IocResolver.Resolve<IImageResizer>();


            OperationPhoto1 = action.Photo1 != OperationPhoto1 ? await imageResizer.ResizeBase64Image(action.Photo1) : OperationPhoto1;
            OperationPhoto2 = action.Photo2 != OperationPhoto2 ? await imageResizer.ResizeBase64Image(action.Photo2) : OperationPhoto2;
            OperationPhoto3 = action.Photo3 != OperationPhoto3 ? await imageResizer.ResizeBase64Image(action.Photo3) : OperationPhoto3;
            OperationPhoto4 = action.Photo4 != OperationPhoto4 ? await imageResizer.ResizeBase64Image(action.Photo4) : OperationPhoto4;

            OperationDate = DateTime.Now;
            OperationLink = action.Link ?? string.Empty;
            OperationNote = action.Note ?? string.Empty;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task SupConfirmProduceAsync(PosmInvestmentSupConfirmProduceAction action)
        {
            if (Status != PosmInvestmentItemStatus.ConfirmedProduce1)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            Status = PosmInvestmentItemStatus.ConfirmedProduce2;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task ProcConfirmProduceAsync(PosmInvestmentSupplyConfirmProduceAction action)
        {
            if (Status != PosmInvestmentItemStatus.ConfirmedProduce2)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            Status = PosmInvestmentItemStatus.ConfirmedVendorProduce;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task SupAcceptAsync(PosmInvestmentSupAcceptAction action)
        {
            if (Status != PosmInvestmentItemStatus.ConfirmedVendorProduce)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            var imageResizer = action.IocResolver.Resolve<IImageResizer>();

            Status = PosmInvestmentItemStatus.Accepted;
            AcceptancePhoto1 = action.Photo1 != Photo1 ? await imageResizer.ResizeBase64Image(action.Photo1) : AcceptancePhoto1;
            AcceptancePhoto2 = action.Photo2 != Photo2 ? await imageResizer.ResizeBase64Image(action.Photo2) : AcceptancePhoto2;
            AcceptancePhoto3 = action.Photo3 != Photo3 ? await imageResizer.ResizeBase64Image(action.Photo3) : AcceptancePhoto3;
            AcceptancePhoto4 = action.Photo4 != Photo4 ? await imageResizer.ResizeBase64Image(action.Photo4) : AcceptancePhoto4;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private async Task SupplyDenyAsync(PosmInvestmentSupplyDenyRequestAction action)
        {
            if (Status != PosmInvestmentItemStatus.DirectorApprovedRequest)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
            Status = PosmInvestmentItemStatus.InvalidOrder;
            PrepareNote = action.Note ?? string.Empty;
            PrepareDate = DateTime.Now;
            VendorId = action.VendorId;
            ActualUnitPrice = action.ActualUnitPrice;
            ActualTotalCost = Math.Round(action.ActualUnitPrice * Qty * PosmValue, 0);
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task SupplyConfirmAsync(PosmInvestmentSupplyConfirmRequestAction action)
        {
            if (Status != PosmInvestmentItemStatus.DirectorApprovedRequest && Status != PosmInvestmentItemStatus.TradeConfirmedUpdateCost)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }
   
            PrepareNote = action.Note ?? string.Empty;
            PrepareDate = DateTime.Now;
            VendorId = action.VendorId;

            if (Status == PosmInvestmentItemStatus.DirectorApprovedRequest)
            {
                ActualUnitPrice = action.ActualUnitPrice;
                ActualTotalCost = Math.Round(action.ActualUnitPrice * Qty * PosmValue, 0);
            }
            Status = PosmInvestmentItemStatus.ValidOrder;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task AsmConfirmAcceptAsync(PosmInvestmentAsmConfirmAcceptAction action)
        {
            if (Status != PosmInvestmentItemStatus.Accepted)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }

            Status = PosmInvestmentItemStatus.ConfirmedAccept1;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }
        private void UpdateSysFields(IIocResolver iocResolver)
        {
            if (Id.IsNew())
            {
                LastModificationTime = DateTime.Now;
                LastModifierUserId = iocResolver.Resolve<ICbmsSession>().UserId;
            }
            
            CreatorUserId = iocResolver.Resolve<ICbmsSession>().UserId;
            CreationTime = DateTime.Now;
        }
        private async Task TradeConfirmAcceptAsync(PosmInvestmentTradeConfirmAcceptAction action)
        {
            if (Status != PosmInvestmentItemStatus.ConfirmedAccept1)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("PosmInvestment.ItemActionInvalid").Build();
            }

            Status = PosmInvestmentItemStatus.ConfirmedAccept2;
            UpdateSysFields(action.IocResolver);
            await LogHistoryAsync(action.IocResolver, action.LocalizationSource);
        }

        private async Task RegisterAsync(PosmInvestmentItemRegisterAction action)
        {
            var imageResizer = action.IocResolver.Resolve<IImageResizer>();

            Photo1 = action.Photo1 != Photo1 ? await imageResizer.ResizeBase64Image(action.Photo1) : Photo1;
            Photo2 = action.Photo2 != Photo2 ? await imageResizer.ResizeBase64Image(action.Photo2) : Photo2;
            Photo3 = action.Photo3 != Photo3 ? await imageResizer.ResizeBase64Image(action.Photo3) : Photo3;
            Photo4 = action.Photo4 != Photo4 ? await imageResizer.ResizeBase64Image(action.Photo4) : Photo4;

            PosmClassId = action.PosmClassId;
            PosmItemId = action.PosmItemId;
            PanelShopName = action.PanelShopName;
            PanelShopPhone = action.PanelShopPhone;
            PanelShopAddress = action.PanelShopAddress;
            PanelOtherInfo = action.PanelOtherInfo;
            PanelProductLine = action.PanelProductLine;
            PosmCatalogId = action.PosmCatalogId;

            Width = action.Width;
            Height = action.Height;
            Depth = action.Depth;
            SideWidth1 = action.SideWidth1;
            SideWidth2 = action.SideWidth2;
            Qty = action.Qty;
            RequestReason = action.RequestReason ?? "";
            RequestType = action.RequestType;

            var posmRepository = action.IocResolver.Resolve<IRepository<PosmItem, int>>();
            var posmItemManager = action.IocResolver.Resolve<IPosmInvestmentManager>();
            var posm = posmRepository.Get(PosmItemId);

            if (posm.CalcType == PosmCalcType.F)
            {
                PosmValue = (SideWidth1.Value + SideWidth2.Value) * Width.Value;
                Size = PosmValue;
            }
            else if (posm.CalcType == PosmCalcType.WH)
            {
                PosmValue = Height.Value * Width.Value;
                Size = PosmValue;
            }
            else if (posm.CalcType == PosmCalcType.HD)
            {
                PosmValue = Height.Value * Depth.Value;
                Size = PosmValue;
            }
            else if (posm.CalcType == PosmCalcType.WHD)
            {
                PosmValue = Height.Value * Width.Value * Depth.Value;
                Size = PosmValue;
            }
            else if (posm.CalcType == PosmCalcType.Q)
            {
                PosmValue = 1;
            }

            UnitPrice = await posmItemManager.GetPriceAsync(PosmItemId);

            TotalCost = PosmValue * UnitPrice * Qty;
            SetupPlanDate = action.SetupPlanDate;
            Status = PosmInvestmentItemStatus.Request;
            CreatorUserId = action.IocResolver.Resolve<ICbmsSession>().UserId;
            CreationTime = DateTime.Now;
            LastModificationTime = DateTime.Now;
            LastModifierUserId = action.IocResolver.Resolve<ICbmsSession>().UserId; ;

        }

        private async Task ImportHistoryAsync(PosmInvestmentItemImportHistoryAction action)
        {
           
            PosmClassId = action.PosmClassId;
            PosmItemId = action.PosmItemId;
            PanelShopName = action.PanelShopName;
            PanelShopPhone = action.PanelShopPhone;
            PanelShopAddress = action.PanelShopAddress;
            PanelOtherInfo = action.PanelOtherInfo;
            PanelProductLine = action.PanelProductLine;
            PosmCatalogId = action.PosmCatalogId;

            Width = action.Width;
            Height = action.Height;
            Depth = action.Depth;
            SideWidth1 = action.SideWidth1;
            SideWidth2 = action.SideWidth2;
            Qty = action.Qty;
            RequestReason = "";
            RequestType = action.RequestType;

           
            
            if (action.CalcType == PosmCalcType.F)
            {
                PosmValue = (SideWidth1.Value + SideWidth2.Value) * Width.Value;
                Size = PosmValue;
            }
            else if (action.CalcType == PosmCalcType.WH)
            {
                PosmValue = Height.Value * Width.Value;
                Size = PosmValue;
            }
            else if (action.CalcType == PosmCalcType.HD)
            {
                PosmValue = Height.Value * Depth.Value;
                Size = PosmValue;
            }
            else if (action.CalcType == PosmCalcType.WHD)
            {
                PosmValue = Height.Value * Width.Value * Depth.Value;
                Size = PosmValue;
            }
            else if (action.CalcType == PosmCalcType.Q)
            {
                PosmValue = 1;
            }

            ActualUnitPrice =  UnitPrice =  action.UnitPrice;
            ActualTotalCost = TotalCost =  PosmValue * UnitPrice * Qty;
            SetupPlanDate = action.SetupPlanDate;
            Status = PosmInvestmentItemStatus.ConfirmedAccept2;
            PrepareDate= action.PrepareDate;
            OperationDate= action.OperationDate;
            AcceptanceDate = action.AcceptanceDate;
            VendorId= action.VendorId;
            CreatorUserId = action.IocResolver.Resolve<ICbmsSession>().UserId;
            CreationTime = DateTime.Now;
            LastModificationTime = DateTime.Now;
            LastModifierUserId = action.IocResolver.Resolve<ICbmsSession>().UserId;

        }

        public async Task LogHistoryAsync(IIocResolver iocResolver, ILocalizationSource localizationSource)
        {
            var history = new PosmInvestmentItemHistory();

            var data = await iocResolver.Resolve<IPosmInvestmentManager>().GetHistoryDataAsync(this); ;

            await history.ApplyActionAsync(new PosmInvestmentItemHistoryUpsertAction(iocResolver, localizationSource, data, Status));

            _histories.Add(history);
        }

        public async Task SalesRemarkAsync(PosmInvestmentSalesRemarkAction action)
        {
            RemarkOfSales = action.Remark;
        }

        public async Task CompanyRemarkAsync(PosmInvestmentCompanyRemarkAction action)
        {
            RemarkOfCompany = action.Remark;
        }
    }
}