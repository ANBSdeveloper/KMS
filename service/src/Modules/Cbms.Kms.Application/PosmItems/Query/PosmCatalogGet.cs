using Cbms.Kms.Application.PosmItems.Dto;
using Cbms.Mediator;
using MediatR;

namespace Cbms.Kms.Application.PosmItems.Query
{
    public class PosmCatalogGet : IRequest<PosmCatalogDto>
    {
        public int Id { get; set; }
        public PosmCatalogGet(int id)
        {
            Id = id;
        }
    }
}
