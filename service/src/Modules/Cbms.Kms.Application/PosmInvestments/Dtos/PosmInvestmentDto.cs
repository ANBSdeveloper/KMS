using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.PosmInvestments;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Dto
{
    [AutoMap(typeof(PosmInvestment))]
    public class PosmInvestmentDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public int CustomerId { get; set; }
        public string CustomerLocationName { get; set; }
        public int CustomerLocationId { get; set; }
        public int RegisterStaffId { get; set; }
        public DateTime RegisterDate { get; set; }
        public int BudgetId { get; set; }
        public decimal CurrentSalesAmount { get; set; }
        public string Note { get; set; }
        public string SetupContact1 { get; set; }
        public string SetupContact2 { get; set; }
        public decimal InvestmentAmount { get; set; }
        public int CycleId { get; set; }
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
        public string ApproveNote { get; set; }
        public decimal CommitmentAmount { get; set; }
        public string CancelReason { get; private set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public decimal? Efficient { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        [IgnoreMap]
        public List<PosmInvestmentItemDto> Items { get; set; }
        [IgnoreMap]
        public List<PosmSalesCommitmentDto> SalesCommitments { get; set; }

    }
}