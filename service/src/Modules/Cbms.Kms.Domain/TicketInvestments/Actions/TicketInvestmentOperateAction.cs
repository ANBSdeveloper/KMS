using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketInvestmentOperateAction : IEntityAction
    {
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }
        public int UserId { get; private set; }
        public DateTime OperationDate { get; private set; }
        public string Note { get; private set; }
        public int StockQuantity { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
        public string Photo5 { get; private set; }
        public bool CompleteOperation { get; private set; }

        public TicketInvestmentOperateAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int userId,
            DateTime operationDate,
            int stockQuantity,
            string photo1,
            string photo2,
            string photo3,
            string photo4,
            string photo5,
            string note,
            bool completeOperation
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            UserId = userId;
            OperationDate = operationDate;
            StockQuantity = stockQuantity;
            Note = note;
            Photo1 = photo1;
            Photo2 = photo2;
            Photo3 = photo3;
            Photo4 = photo4;
            Photo5 = photo5;
            CompleteOperation = completeOperation;
        }
    }
}