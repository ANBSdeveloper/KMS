using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Products.Dto;
using Cbms.Kms.Application.Products.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Kms.Domain.Products;
using Cbms.Mediator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Products.QueryHandlers
{
    public class GetProductByQrCodeHandler : QueryHandlerBase, IRequestHandler<GetProductByQrCode, ProductInfoDto>
    {
        private readonly IProductManager _productFinder;
        private readonly IRepository<InvestmentSetting, int> _investmentSettingRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        public GetProductByQrCodeHandler(
            IRequestSupplement supplement, 
            IProductManager productFinder,
            IRepository<InvestmentSetting, int> investmentSettingRepository,
            IRepository<Customer, int> customerRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _productFinder = productFinder;
            _investmentSettingRepository = investmentSettingRepository;
            _customerRepository = customerRepository;
        }

        public async Task<ProductInfoDto> Handle(GetProductByQrCode request, CancellationToken cancellationToken)
        {
            var customer = _customerRepository.GetAll().FirstOrDefault(p => p.UserId == Session.UserId);
            if (customer == null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Customer.UserIsNotCustomer").Build();
            }

            var productInfo = await _productFinder.CheckAndGetInfoByQrCodeAsync(customer.BranchId.Value, request.QrCode, request.SmallUnitRequire ?? false);

            return Mapper.Map<ProductInfoDto>(productInfo);
        }
    }
}