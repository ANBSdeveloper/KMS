using Cbms.Dependency;
using Cbms.Domain.Repositories;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Customers.Actions;
using Hangfire;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Customers
{
    public class CalculateEfficientJob
    {
        private readonly IIocResolver _iocResolver;
        private readonly IRepository<Customer, int> _customerRepository;

        public CalculateEfficientJob(IIocResolver iocResolver, IRepository<Customer, int> customerRepository)
        {
            _iocResolver = iocResolver;
            _customerRepository = customerRepository;
        }

        [Queue("default")]
        public async Task RunAsync(int customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId);
            await customer.ApplyActionAsync(new CustomerUpdateEfficientAction(_iocResolver));
        }
    }
}