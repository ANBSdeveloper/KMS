using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;
using System;

namespace Cbms.Kms.Domain.TicketInvestments.Actions
{
    public class TicketOperationUpsertAction : IEntityAction
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

        public TicketOperationUpsertAction(
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
            string note
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            UserId = userId;
            Note = note;
            StockQuantity = stockQuantity;
            OperationDate = operationDate;
            Photo1 = photo1;
            Photo2 = photo2;
            Photo3 = photo3;
            Photo4 = photo4;
            Photo5 = photo5;
        }
    }
}