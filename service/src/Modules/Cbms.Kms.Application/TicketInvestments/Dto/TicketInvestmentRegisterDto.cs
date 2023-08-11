using Cbms.Dto;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentRegisterSalesCommitmentDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }

    }

    public class TicketInvestmentRegisterMaterialDto
    {
        public int MaterialId { get; set; }
        public int RegisterQuantity { get; set; }
        public string Note { get; set; }
    }

    public class TicketInvestmentRegisterDto
    {
        public int CustomerId { get; set; }
        public int StockQuantity { get; set; }
        public int RewardPackageId { get; set; }
        public decimal PointsForTicket { get; set; }
        public DateTime BuyBeginDate { get; set; }
        public DateTime BuyEndDate { get; set; }
        public DateTime IssueTicketBeginDate { get; set; }
        public DateTime OperationDate { get; set; }
        public string RegisterNote { get; set; }
        public string SurveyPhoto1 { get; set; }
        public string SurveyPhoto2 { get; set; }
        public string SurveyPhoto3 { get; set; }
        public string SurveyPhoto4 { get; set; }
        public string SurveyPhoto5 { get; set; }
        public List<TicketInvestmentRegisterSalesCommitmentDto> SalesCommitments { get; set; }
        public List<TicketInvestmentRegisterMaterialDto> Materials { get; set; }
    }
}