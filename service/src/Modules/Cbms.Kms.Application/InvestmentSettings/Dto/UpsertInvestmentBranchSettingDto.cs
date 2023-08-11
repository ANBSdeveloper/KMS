using Cbms.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.InvestmentSettings.Dto
{
    public class UpsertInvestmentBranchSettingDto: EntityDto
    {
        public bool IsEditablePoint { get; set; }
        public int BranchId { get; set; }
    }
}
