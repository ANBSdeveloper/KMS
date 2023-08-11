using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentSupplyConfirmRequestCommand : CommandBase
    {
        public class PosmInvestmentSupplyConfirmRequestDto
        {
            public int VendorId { get; set; }
            public string Note { get; set; }
            public decimal ActualUnitPrice { get; set; }
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentSupplyConfirmRequestDto Data { get; set; }

        public PosmInvestmentSupplyConfirmRequestCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
