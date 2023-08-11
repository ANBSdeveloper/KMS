using Cbms.Kms.Application.Staffs.Dto;
using Cbms.Mediator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.Staffs.Query
{
    public class StaffGetByRole : QueryBase, IRequest<StaffListDto>
    {
        public int Id { get; set; }
        public int? SupervisorId { get; set; }
        public string StaffTypeCode { get; set; }
    }
}
