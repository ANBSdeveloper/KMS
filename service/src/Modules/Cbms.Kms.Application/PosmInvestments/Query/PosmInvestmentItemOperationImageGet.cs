using MediatR;

namespace Cbms.Kms.Application.PosmInvestments.Query
{
    public class PosmInvestmentItemOperationImageGet : IRequest<string>
    {
        public int Index { get; set; }
        public int PosmInvestmentItemid { get; set; }
        public PosmInvestmentItemOperationImageGet(int id, int index)
        {
            Index = index;
            PosmInvestmentItemid = id;
        }
    }
}
