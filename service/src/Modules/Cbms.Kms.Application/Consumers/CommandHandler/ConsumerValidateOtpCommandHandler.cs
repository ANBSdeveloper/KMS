using Cbms.Application.Runtime.DistributedLock;
using Cbms.Domain.Entities;
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
    public class ConsumerValidateOtpCommandHandler : RequestHandlerBase, IRequestHandler<ConsumerValidateOtpCommand>
    {
        private readonly IRepository<Consumer, int> _consumerRepository;
        private readonly DistributedLockManager _distributedLockManager;

        public ConsumerValidateOtpCommandHandler(DistributedLockManager distributedLockManager, IRequestSupplement supplement, IRepository<Consumer, int> customerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _consumerRepository = customerRepository;
            _distributedLockManager = distributedLockManager;
        }

        public async Task<Unit> Handle(ConsumerValidateOtpCommand request, CancellationToken cancellationToken)
        {
            var requestData = request.Data;
            await using (await _distributedLockManager.AcquireAsync($"consumer_otp_" + requestData.Phone))
            {
                var consumer = _consumerRepository.GetAll().FirstOrDefault(p => p.Phone == requestData.Phone);
                if (consumer == null)
                {
                    throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Consumer.NotFoundByPhone", requestData.Phone).Build();
                }

                await consumer.ApplyActionAsync(new ConsumerValidateOtpAction(
                    IocResolver,
                    LocalizationSource,
                    requestData.OtpCode
                ));

                return Unit.Value;
            }
        }
    }
}