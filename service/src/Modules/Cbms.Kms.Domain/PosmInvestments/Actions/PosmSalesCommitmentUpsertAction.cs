using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Localization.Sources;

namespace Cbms.Kms.Domain.PosmInvestments.Actions
{
    public class PosmSalesCommitmentUpsertAction : IEntityAction
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public decimal Amount { get; private set; }
        public IIocResolver IocResolver { get; private set; }
        public ILocalizationSource LocalizationSource { get; private set; }

        public PosmSalesCommitmentUpsertAction(
            IIocResolver iocResolver,
            ILocalizationSource localizationSource,
            int year,
            int month,
            decimal amount
        )
        {
            IocResolver = iocResolver;
            LocalizationSource = localizationSource;
            Year = year;
            Month = month;
            Amount = amount;
        }
    }
}