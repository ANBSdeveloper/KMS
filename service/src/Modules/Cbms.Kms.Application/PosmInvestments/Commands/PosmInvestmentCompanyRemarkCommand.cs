using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{
    public class PosmInvestmentRemarkDto
    {
        public int PosmInvestmentId { get; set; }
        public int PosmInvestmentItemId { get; set; }
        public decimal Remark { get; set; }
    }
    public class PosmInvestmentCompanyRemarkCommand : CommandBase
    {
        public PosmInvestmentRemarkDto Data { get; set; }
        public string HandleType { get; set; }

        public PosmInvestmentCompanyRemarkCommand WithId(int id)
        {
            Data.PosmInvestmentId = id;
            return this;
        }
    }
}
