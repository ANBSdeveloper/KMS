using Cbms.Domain.Entities;
using Cbms.Kms.Application.Orders.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Products;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Orders.CommandHandlers
{
    public class OrderValidateSpoonCodeCommandHandler : RequestHandlerBase, IRequestHandler<OrderValidateSpoonCodeCommand>
    {
        private readonly IProductManager _productManager;

        public OrderValidateSpoonCodeCommandHandler(
            IProductManager productManager,
            IRequestSupplement supplement) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _productManager = productManager;
        }

        public async Task<Unit> Handle(OrderValidateSpoonCodeCommand request, CancellationToken cancellationToken)
        {
            var isSpoonValid = await _productManager.CheckSpoonCodeAsync(request.SpoonCode);
            if (!isSpoonValid)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Order.SpoonInValid", request.SpoonCode).Build();
            }

            return Unit.Value;
        }
    }
}