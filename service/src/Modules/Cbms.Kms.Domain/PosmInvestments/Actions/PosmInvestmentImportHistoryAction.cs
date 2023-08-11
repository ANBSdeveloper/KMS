using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System.Collections.Generic;
using System;
using Cbms.Kms.Domain.PosmItems;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{
    public class PosmInvestmentImportHistoryAction : IEntityAction
    {
        public class SalesCommitment
        {
            public int Year { get; private set; }
            public int Month { get; private set; }
            public decimal Amount { get; private set; }

            private SalesCommitment()
            {
            }

            public SalesCommitment(int year, int month, decimal amount)
            {
                Year = year;
                Month = month;
                Amount = amount;
            }
        }

        public class PosmItem
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
            public int Qty { get; private set; }
            public DateTime SetupPlanDate { get; private set; }
            public PosmRequestType RequestType { get; private set; }
            public DateTime PrepareDate { get; set; }
            public int VendorId { get; private set; }
            public DateTime OperationDate { get; private set; }
            public DateTime AcceptanceDate { get; private set; }
            public decimal UnitPrice { get; private set; }
            private PosmItem()
            {

            }
            public PosmItem(
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
                UnitPrice = unitPrice;
                SetupPlanDate = setupPlanDate;
                RequestType = requestType;
                PrepareDate = prepareDate;
                VendorId = vendorId;
                OperationDate = operationDate;
                AcceptanceDate = acceptanceDate;
            }
        }
        public string Code { get; private set; }
        public int CycleId { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public int RegisterStaffId { get; private set; }
        public int CustomerId { get; private set; }
        public int CustomerLocationId { get; private set; }
        public decimal CurrentSalesAmount { get; private set; }
        public string SetupContact1 { get; private set; }
        public string SetupContact2 { get; private set; }
        public string Note { get; private set; }
        public List<SalesCommitment> SalesCommitments { get; private set; }
        public List<PosmItem> Items { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public PosmInvestmentImportHistoryAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int cycleId,
            string code,
            DateTime registerDate,
            int registerStaffId,
            int customerId,
            int customerLocationId,
            decimal currentSalesAmount,
            string setupContact1,
            string setupContact2,
            string note,
            List<SalesCommitment> salesCommitments,
            List<PosmItem> items
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            CycleId = cycleId;
            CustomerId = customerId;
            CustomerLocationId = customerLocationId;
            CurrentSalesAmount = currentSalesAmount;
            RegisterDate= registerDate;
            RegisterStaffId = registerStaffId;
            SetupContact1 = setupContact1;
            SetupContact2 = setupContact2;
            Note = note;
            SalesCommitments = salesCommitments;
            Code = code;
            Items = items;
        }
    }
}