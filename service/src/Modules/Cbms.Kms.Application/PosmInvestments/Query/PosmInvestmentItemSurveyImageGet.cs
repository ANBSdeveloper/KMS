using MediatR;

namespace Cbms.Kms.Application.PosmInvestments.Query
{
    public class PosmInvestmentItemSurveyImageGet : IRequest<string>
    {
        public int Index { get; set; }
        public int PosmInvestmentItemid { get; set; }
        public PosmInvestmentItemSurveyImageGet(int id, int index)
        {
            Index = index;
            PosmInvestmentItemid = id;
        }
    }
}
