using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentSupConfirmProduceCommand : CommandBase
    {
        public class PosmInvestmentSupConfirmProduceDto
        {
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
        }
        public PosmInvestmentSupConfirmProduceDto Data { get; set; }
        public PosmInvestmentSupConfirmProduceCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
