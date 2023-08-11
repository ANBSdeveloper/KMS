using Cbms.Kms.Application.PosmInvestments.Dto;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Mediator;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentRegisterCommand : CommandBase<PosmInvestmentDto>
    {
        public class PosmInvestmentRegisterSalesCommitmentDto
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public decimal Amount { get; set; }
        }
        public class PosmInvestmentRegisterItemDto
        {
            public string PanelShopName { get; set; }
            public string PanelShopPhone { get; set; }
            public string PanelShopAddress { get; set; }
            public string PanelOtherInfo { get; set; }
            public string PanelProductLine { get; set; }
            public int PosmCatalogId { get; set; }
            public decimal? Width { get; set; }
            public decimal? Height { get; set; }
            public decimal? Depth { get; set; }
            public decimal? SideWidth1 { get; set; }
            public decimal? SideWidth2 { get; set; }
            public int Qty { get; set; }
            public decimal UnitPrice { get; set; }
            public DateTime SetupPlanDate { get; set; }
            public PosmRequestType RequestType { get; set; }
            public string RequestReason { get; set; }
            public string Photo1 { get; set; }
            public string Photo2 { get; set; }
            public string Photo3 { get; set; }
            public string Photo4 { get; set; }
        }
        public class PosmInvestmentRegisterDto
        {
            public int CustomerId { get; set; }
            public int CustomerLocationId { get; set; }
            public decimal CurrentSalesAmount { get; set; }
            public string ShopPanelPhoto1 { get; set; }
            public string ShopPanelPhoto2 { get; set; }
            public string ShopPanelPhoto3 { get; set; }
            public string ShopPanelPhoto4 { get; set; }
            public string VisibilityPhoto1 { get; set; }
            public string VisibilityPhoto2 { get; set; }
            public string VisibilityPhoto3 { get; set; }
            public string VisibilityPhoto4 { get; set; }
            public string VisibilityCompetitorPhoto1 { get; set; }
            public string VisibilityCompetitorPhoto2 { get; set; }
            public string VisibilityCompetitorPhoto3 { get; set; }
            public string VisibilityCompetitorPhoto4 { get; set; }
            public string Note { get; set; }
            public string SetupContact1 { get; set; }
            public string SetupContact2 { get; set; }
            public List<PosmInvestmentRegisterSalesCommitmentDto> SalesCommitments { get; set; }
            public List<PosmInvestmentRegisterItemDto> Items { get; set; }
        }

        public PosmInvestmentRegisterDto Data { get; set; }
    }
}
