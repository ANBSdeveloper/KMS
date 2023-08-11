using Cbms.Dto;
namespace Cbms.Kms.Application.CustomerSalesItems.Dto
{
    public class CustomerSalesItemCreateDto : AuditedEntityDto
    {
        public string QrCode { get; set; }
    }
}