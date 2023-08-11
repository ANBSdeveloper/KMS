using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentSupplyConfirmProduceCommand : CommandBase
    {
        public class PosmInvestmentSupplyConfirmProduceDto
        {
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentSupplyConfirmProduceDto Data { get; set; }
        public PosmInvestmentSupplyConfirmProduceCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
