using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{
    public class PosmInvestmentSalesRemarkCommand : CommandBase
    {
        public PosmInvestmentRemarkDto Data { get; set; }
        public string HandleType { get; set; }

        public PosmInvestmentSalesRemarkCommand WithId(int id)
        {
            Data.PosmInvestmentId = id;
            return this;
        }
    }
}
