using Cbms.Kms.Application.Branches.Dto;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Branches.Query
{
    public class GetBranchList : EntityPagingResultQuery<BranchListItemDto>
    {
        public bool? IsActive { get; set; }
    }
}