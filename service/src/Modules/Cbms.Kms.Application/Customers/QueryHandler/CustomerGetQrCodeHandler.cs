using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Customers.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Customers;
using Cbms.Mediator;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers.QueryHandler
{
    public class CustomerGetQrCodeHandler : QueryHandlerBase, IRequestHandler<CustomerGetQrData, string>
    {
        private readonly IRepository<Customer, int> _repository;
        private readonly IAppSettingManager _appSettingManager;

        public CustomerGetQrCodeHandler(IAppSettingManager appSettingManager, IRequestSupplement supplement, IRepository<Customer, int> repository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _repository = repository;
            _appSettingManager = appSettingManager;
        }

        public async Task<string> Handle(CustomerGetQrData request, CancellationToken cancellationToken)
        {
            var customer =  await _repository.FirstOrDefaultAsync(p=>p.Code == request.Code);
            if (customer == null)
            {
                throw new EntityNotFoundException(typeof(Customer), request.Code);
            }
            string qrLink = await _appSettingManager.GetAsync("QRCODE_LINK");

            return qrLink + "/" + customer.Code;

        }
    }
}