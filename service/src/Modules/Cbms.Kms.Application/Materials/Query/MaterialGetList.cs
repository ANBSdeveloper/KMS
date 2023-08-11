using Cbms.Kms.Application.Materials.Dto;
using Cbms.Mediator;

namespace Cbms.Kms.Application.Materials.Query
{
    public class MaterialGetList : EntityPagingResultQuery<MaterialListItemDto>
    {
        public bool? IsActive { get; set; }
        public int? MaterialTypeId { get; set; }
    }
}
