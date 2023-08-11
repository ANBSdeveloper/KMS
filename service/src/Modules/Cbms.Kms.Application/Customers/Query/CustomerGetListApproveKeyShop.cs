using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Customers.Query
{
    public class CustomerGetListApproveKeyShop : EntityPagingResultQuery<CustomerApproveKeyShopListDto>
    {
        public int ZoneId { get; set; }
        public int AreaId { get; set; }
    }
}
