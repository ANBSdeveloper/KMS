using Cbms.Kms.Application.Products.Dto;
using Cbms.Kms.Application.Products.Query;
using Cbms.Kms.Application.Vendors.Dto;
using Cbms.Kms.Application.Vendors.Query;
using Cbms.Kms.Domain.Vendors;
using Cbms.Kms.Infrastructure;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Cbms.Mediator.Query;

namespace Cbms.Kms.Application.Vendors.QueryHandler
{

    public class VendorGetListHandler : QueryHandlerBase, IRequestHandler<VendorGetList, PagingResult<VendorListDto>>
    {
        private readonly AppDbContext _dbContext;
        public VendorGetListHandler(IRequestSupplement supplement, AppDbContext dbContext) : base(supplement)
        {
            _dbContext = dbContext;
        }

        public async Task<PagingResult<VendorListDto>> Handle(VendorGetList request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword;
            var query = from vendor in _dbContext.Vendors
                        join zone in _dbContext.Zones on vendor.ZoneId equals zone.Id into zonel
                        from zone in zonel.DefaultIfEmpty()
                        select new VendorListDto()
                        {
                            ZoneId = vendor.ZoneId,
                            Phone = vendor.Name,
                            Address = vendor.Address,
                            Representative= vendor.Representative,
                            TaxReg = vendor.TaxReg,
                            Zone = zone.Name,
                            Code = vendor.Code,
                            CreationTime = vendor.CreationTime,
                            CreatorUserId = vendor.CreatorUserId,
                            Id = vendor.Id,
                            IsActive = vendor.IsActive,
                            LastModificationTime = vendor.LastModificationTime,
                            LastModifierUserId = vendor.LastModifierUserId,
                            Name = vendor.Name
                        };

            query = query
                .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive)
                .WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) ||
                   EF.Functions.Like(x.Name, $"{keyword}%") || EF.Functions.Like(x.Address, $"{keyword}%"));

            int totalCount = query.Count();
            query = query.SortFromString(request.Sort);
            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.MaxResult.HasValue)
            {
                query = query.Take(request.MaxResult.Value);
            }
            return new PagingResult<VendorListDto>()
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
        }
    }
}