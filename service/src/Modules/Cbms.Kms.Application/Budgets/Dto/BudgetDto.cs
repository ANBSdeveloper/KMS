using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Budgets;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Budgets.Dto
{
    [AutoMap(typeof(Budget))]
    public class BudgetDto : AuditedEntityDto
    {
        public int CycleId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int InvestmentType { get; set; }
        public List<BudgetZoneDto> Zones { get; set; }
        public List<BudgetAreaDto> Areas { get; set; }
        public List<BudgetBranchDto> Branches { get; set; }
    }
}