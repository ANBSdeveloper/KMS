using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Repositories;
using Cbms.EntityFrameworkCore;
using Cbms.Kms.Application.Orders.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Domain.Orders.Actions;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static Cbms.Kms.Domain.Orders.Actions.OrderUpdateSpoonCodeAction;

namespace Cbms.Kms.Application.Orders.CommandHandlers
{
    public class OrderUpdateSpoonCodeCommandHandler : RequestHandlerBase, IRequestHandler<OrderUpdateSpoonCodeCommand, OrderUpdateSpoonResult>
    {
        private readonly IRepository<Order, int> _orderRepository;
        private readonly DistributedLockManager _distributedLockManager;
        private readonly IDbContextProvider _contextProvider;
        private readonly IAppLogger _appLogger;
        public OrderUpdateSpoonCodeCommandHandler(
            DistributedLockManager distributedLockManager, 
            IRequestSupplement supplement,
            IDbContextProvider contextProvider,
        IAppLogger appLogger,
            IRepository<Order, int> orderRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _orderRepository = orderRepository;
            _contextProvider = contextProvider;
            _distributedLockManager = distributedLockManager;
            _appLogger = appLogger;
        }

        public async Task<OrderUpdateSpoonResult> Handle(OrderUpdateSpoonCodeCommand request, CancellationToken cancellationToken)
        {
            await using (await _distributedLockManager.AcquireAsync($"order"))
            {
                await _appLogger.LogInfoAsync("UPDATE_SPOON", new { Data = request, Message = "Request" });

                var order = new Order();

                OrderUpdateSpoonResult result = null;

                await order.ApplyActionAsync(new OrderUpdateSpoonCodeAction(
                    IocResolver,
                    LocalizationSource,
                    request.Phone,
                    request.Name,
                    request.QrCode,
                    request.SpoonCode,
                    (r) => result = r));

                if (result.Result == 0 || result.Result == 1 || result.Result == 6)
                {
                    
                    await _orderRepository.InsertAsync(order);
                    await _orderRepository.UnitOfWork.CommitAsync();
                }
                else
                {
                    _contextProvider.GetDbContext().Clear();
                }

                await _appLogger.LogInfoAsync("UPDATE_SPOON", new { Data = request, Message = "Complete" });

                

                return result;
            }
            
        }
    }
}