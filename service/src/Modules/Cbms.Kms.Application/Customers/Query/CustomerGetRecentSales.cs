using Cbms.Kms.Application.Customers.Dto;
using Cbms.Mediator;
using MediatR;
using System;

namespace Cbms.Kms.Application.Customers.Query
{
    public class CustomerGetRecentSales : QueryBase,IRequest<CustomerRecentSalesDto>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}

