using Cbms.Mediator;

namespace Cbms.Kms.Application.PosmInvestments.Commands
{

    public class PosmInvestmentSupAcceptCommand : CommandBase
    {
        public class PosmInvestmentSupAcceptDto
        {
            public int PosmInvestmentItemId { get; set; }
            public int Id { get; set; }
            public string Note { get;  set; }
            public string Photo1 { get;  set; }
            public string Photo2 { get;  set; }
            public string Photo3 { get;  set; }
            public string Photo4 { get;  set; }
        }
        public PosmInvestmentSupAcceptDto Data { get; set; }
        public PosmInvestmentSupAcceptCommand WithId(int id)
        {
            Data.Id = id;
            return this;
        }
    }
}
