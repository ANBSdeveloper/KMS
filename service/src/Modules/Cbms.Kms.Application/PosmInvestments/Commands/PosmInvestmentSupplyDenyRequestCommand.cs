using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentSupplyDenyRequestCommand : CommandBase
    {
        public class PosmInvestmentSupplyDenyRequestDto
        {
            public int VendorId { get; set; }
            public string Note { get; set; }
            public decimal ActualUnitPrice { get; set; }
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentSupplyDenyRequestDto Data { get; set; }

        public PosmInvestmentSupplyDenyRequestCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
