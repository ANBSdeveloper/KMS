using Cbms.Kms.Application.SubProductClasses.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.SubProductClasses.Query
{
    public class SubProductClassGetList : EntityPagingResultQuery<SubProductClassDto>
    {
        public bool? IsActive { get; set; }
    }
}