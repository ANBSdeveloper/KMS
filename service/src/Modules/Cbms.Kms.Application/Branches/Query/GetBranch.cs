using Cbms.Kms.Application.Branches.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Branches.Query
{
    public class GetBranch : EntityQuery<BranchDto>
    {
        public GetBranch(int id) : base(id)
        {
        }
    }
}