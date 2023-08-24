using Cbms.Dto;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmItems;
using System;

namespace Cbms.Kms.Application.PosmInvestments.Dto
{
    public class PosmInvestmentItemDto : AuditedEntityDto
    {
        public int PosmClassId { get; set; }
        public int PosmItemId { get; set; }
        public string PosmItemCode { get; set; }
        public string PosmItemName { get; set; }
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
        public decimal? ActualUnitPrice { get; set; }
        public decimal TotalCost { get; set; }
        public decimal? ActualTotalCost { get; set; }
        public decimal? Size { get; set; }
        public decimal PosmValue { get; set; }
        public DateTime SetupPlanDate { get; set; }
        public PosmInvestmentItemStatus Status { get; set; }
        public PosmRequestType RequestType { get; set; }
        public PosmUnitType UnitType { get; set; }
        public PosmCalcType CalcType { get; set; }
        public bool InclueInfo { get; set; }
        public string RequestReason { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public int PosmInvestmentId { get; set; }
        public string PrepareNote { get; set; }
        public DateTime? PrepareDate { get; set; }
        public string UpdateCostReason { get; set; }
        public string OperationPhoto1 { get; set; }
        public string OperationPhoto2 { get; set; }
        public string OperationPhoto3 { get; set; }
        public string OperationPhoto4 { get; set; }
        public string OperationLink { get; set; }
        public string OperationNote { get; set; }

        public DateTime? OperationDate { get; set; }

        public string AcceptancePhoto1 { get; set; }
        public string AcceptancePhoto2 { get; set; }
        public string AcceptancePhoto3 { get; set; }
        public string AcceptancePhoto4 { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public string AcceptanceNote { get; set; }
        public int? VendorId { get; set; }
        public decimal? RemarkOfSales { get; set; }
        public decimal? RemarkOfCompany { get; set; }
		public string DesignPhoto1 { get; set; }
		public string DesignPhoto2 { get; set; }
		public string DesignPhoto3 { get; set; }
		public string DesignPhoto4 { get; set; }
	}
}