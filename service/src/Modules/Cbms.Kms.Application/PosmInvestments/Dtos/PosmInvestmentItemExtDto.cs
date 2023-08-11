using Cbms.Dto;
using Cbms.Kms.Domain.PosmInvestments;
using System;

namespace Cbms.Kms.Application.PosmInvestments.Dto
{
    public class PosmInvestmentItemExtDto
    {
        public string Code { get; set; }
        public DateTime RegisterDate { get; set; }
        public int PosmInvestmentItemId { get; set; }
        public string PosmItemCode { get; set; }
        public string PosmItemName { get; set; }
        public decimal InvestmentAmount { get; set; }
        public PosmInvestmentItemStatus ItemStatus { get; set; }
        public PosmInvestmentStatus Status { get; set; }
        public string PosmInvestmentCode { get; set; }
        public int PosmInvestmentId { get; set; }
        public int PosmItemId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public decimal? ActualTotalCost { get; set; }
        public decimal TotalCost { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }

    }
}