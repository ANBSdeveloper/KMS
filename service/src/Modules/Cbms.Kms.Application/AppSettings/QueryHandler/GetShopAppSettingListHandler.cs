using Cbms.Domain.Repositories;
using Cbms.Kms.Application.AppSettings.Dto;
using Cbms.Kms.Application.AppSettings.Query;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Mediator;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.AppSettings.QueryHandler
{
    public class GetShopAppSettingListHandler : QueryHandlerBase, IRequestHandler<GetShopAppSettingList, List<ShopAppSettingDto>>
    {
        private IRepository<AppSetting, int> _appSettingRepository;
        public GetShopAppSettingListHandler(IRequestSupplement supplement, IRepository<AppSetting, int> appSettingRepository) : base(supplement)
        {
            _appSettingRepository = appSettingRepository;
        }

        public async Task<List<ShopAppSettingDto>> Handle(GetShopAppSettingList request, CancellationToken cancellationToken)
        {
            return _appSettingRepository
                .GetAll()
                .Where(p => p.Code == "REGISTER_NEW_SHOP")
                .Select(p => new ShopAppSettingDto() { Code = p.Code, Value = p.Value})
                .ToList();
        }
    }
}
