using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Consumers.Dto;
using Cbms.Kms.Application.Customers.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Consumers;
using Cbms.Kms.Domain.Customers.Actions;
using Cbms.Mediator;
using MediatR;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.QueryHandler
{
    public class ConsumerGetByPhoneHandler : QueryHandlerBase, IRequestHandler<ConsumerGetByPhone, ConsumerInfoDto>
    {
        private readonly IConsumerFinder _consumerFinder;
        private readonly IRepository<Consumer, int> _consumerRepository;
        public ConsumerGetByPhoneHandler(
            IRequestSupplement supplement, 
            IConsumerFinder consumerFinder, 
            IRepository<Consumer, int> consumerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _consumerFinder = consumerFinder;
            _consumerRepository = consumerRepository;
        }

        public async Task<ConsumerInfoDto> Handle(ConsumerGetByPhone request, CancellationToken cancellationToken)
        {
            Regex regex = new Regex("^[0-9]{9,15}$");
            if (!regex.IsMatch(request.Phone ?? ""))
            {
                var exp = BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Consumer.PhoneInvalid").Build();
                exp.Data.Add("phoneInvalid", 1);
                throw exp;
            }
            
            var consumerInfo = await _consumerFinder.FindByPhoneAsync(request.Phone);
            if (consumerInfo == null)
            {
                consumerInfo = await _consumerFinder.FindByPhoneInSalesForce(request.Phone);
                if (consumerInfo != null)
                {
                    var consumer = new Consumer();
                    await consumer.ApplyActionAsync(new ConsumerCreateAction(consumerInfo.Phone, consumerInfo.Name));
                    await _consumerRepository.InsertAsync(consumer);
                    await _consumerRepository.UnitOfWork.CommitAsync();
                }
            }
            if (consumerInfo == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Consumer.NotFoundByPhone", request.Phone).Build();
            }
            return Mapper.Map<ConsumerInfoDto>(consumerInfo);
        }
    }
}