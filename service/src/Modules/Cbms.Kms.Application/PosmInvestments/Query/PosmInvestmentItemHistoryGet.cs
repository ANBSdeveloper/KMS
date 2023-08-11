using Cbms.Kms.Domain.PosmInvestments;
using MediatR;
using System;
using System.Collections.Generic;

namespace Cbms.Kms.Application.PosmInvestments.Query
{

    public class PosmInvestmnetItemHistoryItemDto
    {
        public int Id { get; set; }
        public PosmInvestmentItemStatus Status { get; set; }
        public string Data { get; set; }
        public string CreationUser { get; set; }
        public string CreationRole { get; set; }
        public DateTime CreationTime { get; set; }
        public string LastModifierUser { get; set; }
        public decimal? RemarkOfSales { get; set; }
        public decimal? RemarkOfCompany { get; set; }
        public PosmInvestmnetItemHistoryItemDto()
        {
            LastModifierUser = string.Empty;
        }
    }
    public class PosmInvestmentItemHistoryDto
    {
        public List<PosmInvestmnetItemHistoryItemDto> RequestItems { get; set; }
        public List<PosmInvestmnetItemHistoryItemDto> ApproveItems { get; set; }
        public List<PosmInvestmnetItemHistoryItemDto> PrepareItems { get; set; }
        public List<PosmInvestmnetItemHistoryItemDto> OperationItems { get; set; }
        public List<PosmInvestmnetItemHistoryItemDto> AcceptanceItems { get; set; }

        public PosmInvestmentItemHistoryDto()
        {
            RequestItems = new List<PosmInvestmnetItemHistoryItemDto>();
            ApproveItems = new List<PosmInvestmnetItemHistoryItemDto>();
            PrepareItems = new List<PosmInvestmnetItemHistoryItemDto>();
            OperationItems = new List<PosmInvestmnetItemHistoryItemDto>();
            AcceptanceItems = new List<PosmInvestmnetItemHistoryItemDto>();
        }
    }
    public class PosmInvestmentItemHistoryGet : IRequest<PosmInvestmentItemHistoryDto>
    {
        public int Id { get; set; }
        public PosmInvestmentItemHistoryGet(int id)
        {
            Id = id;
        }
    }
}