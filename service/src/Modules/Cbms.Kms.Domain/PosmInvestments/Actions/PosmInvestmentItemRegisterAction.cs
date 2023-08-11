﻿using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{
    public class PosmInvestmentItemRegisterAction : IEntityAction
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
        public DateTime SetupPlanDate { get; private set; }
        public PosmRequestType RequestType { get; private set; }
        public string RequestReason { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
      
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public PosmInvestmentItemRegisterAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int posmClassId,
            int posmItemId,
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
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            PosmClassId = posmClassId;
            PosmItemId = posmItemId;
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
}