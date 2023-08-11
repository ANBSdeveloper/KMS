using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{
    public class PosmInvestmentItemImportHistoryAction : IEntityAction
    {
        public int PosmItemId { get; private set; }
        public int PosmClassId { get; private set; }
        public PosmCalcType CalcType { get; private set; }
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
        public decimal UnitPrice { get; private set; }
        public int Qty { get; private set; }
        public DateTime SetupPlanDate { get; private set; }
        public PosmRequestType RequestType { get; private set; }
        public DateTime PrepareDate { get; set; }
        public int VendorId { get; private set; }
        public DateTime OperationDate { get; private set; }
        public DateTime AcceptanceDate { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public PosmInvestmentItemImportHistoryAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmItemId,
            int posmClassId,
            PosmCalcType calcType,
            string panelShopName,
            string panelShopPhone,
            string panelShopAddress,
            string panelOtherInfo,
            string panelProductLine,
            int posmCatalogId,
            decimal? width,
            decimal? height,
            decimal? depth,
            decimal? sideWidth1,
            decimal? sideWidth2,
            int qty,
            decimal unitPrice,
            DateTime setupPlanDate,
            PosmRequestType requestType,
            DateTime prepareDate,
            int vendorId,
            DateTime operationDate,
            DateTime acceptanceDate
            )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            PosmItemId = posmItemId;
            PosmClassId = posmClassId;
            CalcType = calcType;
            PanelShopName = panelShopName;
            PanelShopPhone = panelShopPhone;
            PanelShopAddress = panelShopAddress;
            PanelOtherInfo = panelOtherInfo;
            PanelProductLine = panelProductLine;
            PosmCatalogId = posmCatalogId;
            Width = width;
            Height = height;
            Depth = depth;
            SideWidth1 = sideWidth1;
            SideWidth2 = sideWidth2;
            Qty = qty;
            SetupPlanDate = setupPlanDate;
            RequestType = requestType;
            PrepareDate = prepareDate;
            VendorId = vendorId;
            OperationDate = operationDate;
            AcceptanceDate = acceptanceDate;
            UnitPrice = unitPrice;
        }
    }
}