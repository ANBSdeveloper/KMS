using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketInvestmentRegisterSalesCommitment
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public decimal Amount { get; private set; }
        private TicketInvestmentRegisterSalesCommitment()
        {

        }
        public TicketInvestmentRegisterSalesCommitment(int year, int month, decimal amount)
        {
            Year = year;
            Month = month;
            Amount = amount;
        }
    }

    public class TicketInvestmentRegisterMaterial
    {
        public int MaterialId { get; private set; }
        public int RegisterQuantity { get; private set; }
        public string Note { get; private set; }
        private TicketInvestmentRegisterMaterial()
        {

        }
        public TicketInvestmentRegisterMaterial(int materialId, int registerQuantity, string note)
        {
            MaterialId = materialId;
            RegisterQuantity = registerQuantity;
            Note = note;
        }
    }

    public class TicketInvestmentRegisterAction : IEntityAction
    {
        public int CustomerId { get; private set; }
        public int StockQuantity { get; private set; }
        public int RewardPackageId { get; private set; }
        public decimal PointsForTicket { get; private set; }
        public DateTime BuyBeginDate { get; private set; }
        public DateTime BuyEndDate { get; private set; }
        public DateTime IssueTicketBeginDate { get; private set; }
        public DateTime OperationDate { get; private set; }
        public string RegisterNote { get; private set; }
        public string SurveyPhoto1 { get; private set; }
        public string SurveyPhoto2 { get; private set; }
        public string SurveyPhoto3 { get; private set; }
        public string SurveyPhoto4 { get; private set; }
        public string SurveyPhoto5 { get; private set; }
        public int UserId { get; private set; }
        public List<TicketInvestmentRegisterSalesCommitment> SaleCommitments { get; private set; }
        public List<TicketInvestmentRegisterMaterial> Materials { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public TicketInvestmentRegisterAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int userId,
            int customerId,
            int stockQuantity,
            int rewardPackageId,
            decimal pointsForTicket,
            DateTime buyBeginDate,
            DateTime buyEndDate,
            DateTime issueTicketBeginDate,
            DateTime operationDate,
            string registerNote,
            string surveyPhoto1,
            string surveyPhoto2,
            string surveyPhoto3,
            string surveyPhoto4,
            string surveyPhoto5,
            List<TicketInvestmentRegisterSalesCommitment> salesCommitments,
            List<TicketInvestmentRegisterMaterial> materials
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            UserId = userId;
            CustomerId = customerId;
            StockQuantity = stockQuantity;
            RewardPackageId = rewardPackageId;
            PointsForTicket = pointsForTicket;
            BuyBeginDate = buyBeginDate;
            BuyEndDate = buyEndDate;
            IssueTicketBeginDate = issueTicketBeginDate;
            OperationDate = operationDate;
            RegisterNote = registerNote;
            SaleCommitments = salesCommitments;
            Materials = materials;
            SurveyPhoto1 = surveyPhoto1;
            SurveyPhoto2 = surveyPhoto2;
            SurveyPhoto3 = surveyPhoto3;
            SurveyPhoto4 = surveyPhoto4;
            SurveyPhoto5 = surveyPhoto5;
        }
    }
}