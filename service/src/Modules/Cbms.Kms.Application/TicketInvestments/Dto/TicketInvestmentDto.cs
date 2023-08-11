using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.TicketInvestments;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    [AutoMap(typeof(TicketInvestment))]
    public class TicketInvestmentDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal? Efficient { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public int RegisterStaffId { get; set; }
        public int StockQuantity { get; set; }
        public int RewardPackageId { get; set; }
        public int BudgetId { get; set; }
        public int CycleId { get; set; }
        public int TicketQuantity { get; set; }
        public decimal PointsForTicket { get; set; }
        public decimal SalesPlanAmount { get; set; }
        public decimal CommitmentAmount { get; set; }
        public decimal RewardAmount { get; set; }
        public decimal MaterialAmount { get; set; }
        public decimal InvestmentAmount { get; set; }
        public DateTime BuyBeginDate { get; set; }
        public DateTime BuyEndDate { get; set; }
        public DateTime IssueTicketBeginDate { get; set; }
        public DateTime IssueTicketEndDate { get; set; }
        public int Status { get; set; }
        public DateTime OperationDate { get; set; }
        public string RegisterNote { get; set; }
        public string SurveyPhoto1 { get; set; }
        public string SurveyPhoto2 { get; set; }
        public string SurveyPhoto3 { get; set; }
        public string SurveyPhoto4 { get; set; }
        public string SurveyPhoto5 { get; set; }
        public int PrintTicketQuantity { get; set; }
        public int SmsTicketQuantity { get; set; }
        public decimal ActualSalesAmount { get; set; }
        [IgnoreMap]
        public List<TicketMaterialDto> Materials { get; set; }
        [IgnoreMap]
        public List<TicketRewardItemDto> RewardItems { get; set; }
        [IgnoreMap]
        public List<TicketSalesCommitmentDto> SalesCommitments { get; set; }
        [IgnoreMap]
        public List<TicketConsumerRewardDto> ConsumerRewards { get; set; }
        [IgnoreMap]
        public List<TicketProgressDto> Progresses { get; set; }
        [IgnoreMap]
        public TicketAcceptanceDto Acceptance { get; set; }
        [IgnoreMap]
        public TicketOperationDto Operation { get; set; }
        [IgnoreMap]
        public TicketFinalSettlementDto FinalSettlement { get; set; }
    }
}