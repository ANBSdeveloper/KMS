using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Cycles;
using System;

namespace Cbms.Kms.Application.Cycles.Dto
{
    [AutoMap(typeof(Cycle))]
    public class CycleDto : AuditedEntityDto
    {
        public string Number { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}