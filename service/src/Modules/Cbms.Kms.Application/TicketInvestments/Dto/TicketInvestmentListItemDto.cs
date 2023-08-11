using Cbms.Dto;
using System;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentListItemDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public int CycleId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get;  set; }
        public string CustomerName { get;  set; }
        public string Email { get;  set; }
        public string MobilePhone { get;  set; }
        public string Address { get;  set; }
        public int RegisterStaffId { get; set; }
        public string RegisterStaffName { get; set; }
        public int StockQuantity { get; set; }
        public int RewardPackageId { get; set; }
        public string RewardPackageName { get; set; }
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
        public decimal? RemarkOfCompany { get; set; }
        public string ZoneName { get; set; }
        public string AreaName { get; set; }
    }
}