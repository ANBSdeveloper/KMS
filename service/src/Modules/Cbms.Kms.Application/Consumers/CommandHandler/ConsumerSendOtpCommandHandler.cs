using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Consumers.Commands;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Consumers;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Mediator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Consumers.CommandHandlers
{
    public class ConsumerSendOtpCommandHandler : RequestHandlerBase, IRequestHandler<ConsumerSendOtpCommand>
    {
        private readonly IRepository<Consumer, int> _consumerRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public ConsumerSendOtpCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<Consumer, int> customerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _consumerRepository = customerRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(ConsumerSendOtpCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            await using (await _distributedLockManager.AcquireAsync($"consumer_otp_" + request.Data.Phone))
            {
                var consumer = _consumerRepository.GetAll().FirstOrDefault(p => p.Phone == request.Data.Phone);
                if (consumer == null)
                {
                    consumer = new Consumer();
                    await _consumerRepository.InsertAsync(consumer);
                }

                await consumer.ApplyActionAsync(new ConsumerSendOtpAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.Phone
                ));

                return Unit.Value;
            }
        }
    }
}