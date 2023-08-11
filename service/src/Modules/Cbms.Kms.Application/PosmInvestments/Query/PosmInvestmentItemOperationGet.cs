using Cbms.Dto;
using MediatR;
using System;

namespace Cbms.Kms.Application.PosmInvestments.Query
{
    public class PosmInvestmentItemOperationDto: EntityDto
    {
        public int PosmCatalogId { get; set; }
        public string OperationPhoto1 { get; set; }
        public string OperationPhoto2 { get; set; }
        public string OperationPhoto3 { get; set; }
        public string OperationPhoto4 { get; set; }
        public string OperationLink { get; set; }
        public string OperationNote { get; set; }
        public DateTime? OperationDate { get; set; }
        public string ConfirmUserName { get; set; }
    }
    public class PosmInvestmentItemOperationGet : IRequest<PosmInvestmentItemOperationDto>
    {
        public int Id { get; private set; }
        public PosmInvestmentItemOperationGet(int id)
        {
            Id = id;
        }
    }
}