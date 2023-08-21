using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{

    public class PosmInvestmentRegisterAction : IEntityAction
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
            public string RequestReason { get; private set; }
            public string Photo1 { get; private set; }
            public string Photo2 { get; private set; }
            public string Photo3 { get; private set; }
            public string Photo4 { get; private set; }
            private PosmItem()
            {

            }
            public PosmItem(
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
                DateTime setupPlanDate,
                PosmRequestType requestType,
                string requestReason,
                string photo1,
                string photo2,
                string photo3,
                string photo4
            )
            {
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
                RequestReason = requestReason;
                Photo1 = photo1;
                Photo2 = photo2;
                Photo3 = photo3;
                Photo4 = photo4;
            }
        }

        public int CustomerId { get; private set; }
        public int CustomerLocationId { get; private set; }
        public decimal CurrentSalesAmount { get; private set; }
		public decimal VTDCommitmentAmount { get; private set; }
		public decimal MilkIndustryAmount { get; private set; }
		public string ShopPanelPhoto1 { get; private set; }
        public string ShopPanelPhoto2 { get; private set; }
        public string ShopPanelPhoto3 { get; private set; }
        public string ShopPanelPhoto4 { get; private set; }
        public string VisibilityPhoto1 { get; private set; }
        public string VisibilityPhoto2 { get; private set; }
        public string VisibilityPhoto3 { get; private set; }
        public string VisibilityPhoto4 { get; private set; }
        public string VisibilityCompetitorPhoto1 { get; private set; }
        public string VisibilityCompetitorPhoto2 { get; private set; }
        public string VisibilityCompetitorPhoto3 { get; private set; }
        public string VisibilityCompetitorPhoto4 { get; private set; }
        public string SetupContact1 { get; private set; }
        public string SetupContact2 { get; private set; }
        public string Note { get; private set; }
        public int UserId { get; private set; }
        public List<SalesCommitment> SalesCommitments { get; private set; }
        public List<PosmItem> Items { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public PosmInvestmentRegisterAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int userId,
            int customerId,
            int customerLocationId,
            decimal currentSalesAmount,
            string shopPanelPhoto1,
            string shopPanelPhoto2,
            string shopPanelPhoto3,
            string shopPanelPhoto4,
            string visibilityPhoto1,
            string visibilityPhoto2,
            string visibilityPhoto3,
            string visibilityPhoto4,
            string visibilityCompetitorPhoto1,
            string visibilityCompetitorPhoto2,
            string visibilityCompetitorPhoto3,
            string visibilityCompetitorPhoto4,
            string setupContact1,
            string setupContact2,
            string note,
            List<SalesCommitment> salesCommitments,
            List<PosmItem> items,
			decimal vtdCommitmentAmount,
			decimal milkIndustryAmount
		)
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            CustomerId = customerId;
            CustomerLocationId = customerLocationId;
            CurrentSalesAmount = currentSalesAmount;
            ShopPanelPhoto1 = shopPanelPhoto1;
            ShopPanelPhoto2 = shopPanelPhoto2;
            ShopPanelPhoto3 = shopPanelPhoto3;
            ShopPanelPhoto4 = shopPanelPhoto4;
            VisibilityPhoto1 = visibilityPhoto1;
            VisibilityPhoto2 = visibilityPhoto2;
            VisibilityPhoto3 = visibilityPhoto3;
            VisibilityPhoto4 = visibilityPhoto4;
            VisibilityCompetitorPhoto1 = visibilityCompetitorPhoto1;
            VisibilityCompetitorPhoto2 = visibilityCompetitorPhoto2;
            VisibilityCompetitorPhoto3 = visibilityCompetitorPhoto3;
            VisibilityCompetitorPhoto4 = visibilityCompetitorPhoto4;
            SetupContact1 = setupContact1;
            SetupContact2 = setupContact2;
            Note = note;
            UserId = userId;
            SalesCommitments = salesCommitments;
            Items = items;
            VTDCommitmentAmount = vtdCommitmentAmount;
            MilkIndustryAmount = milkIndustryAmount;
        }
    }
}