using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Kms.Application.Staffs.Query;
using Cbms.Kms.Domain.Staffs;
using Cbms.Linq.Extensions;
using Cbms.Mediator;
using System.Linq;

namespace Cbms.Kms.Application.Staffs.QueryHandler
{
    public class StaffGetStaffListHandler : EntityPagedQueryHandler<StaffGetList, int, Staff, StaffListDto>
    {
        public StaffGetStaffListHandler(IRequestSupplement supplement) : base(supplement)
        {
        }

        protected override IQueryable<Staff> Filter(IQueryable<Staff> query, StaffGetList request)
        {
            var keyword = request.Keyword;
            return query.WhereIf(!string.IsNullOrEmpty(request.Keyword), x => x.Code.Contains(keyword) || x.Name.Contains(keyword) || x.MobilePhone.Contains(keyword));
        }
    }
}
