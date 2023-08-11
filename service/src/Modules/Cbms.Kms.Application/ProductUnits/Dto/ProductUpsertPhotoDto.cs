using Cbms.Dto;
using Microsoft.AspNetCore.Http;

namespace WMSLight.Inventory.Products.Dto
{
    public class ProductUpsertPhotoDto : EntityDto<int>
    {
        public IFormFile File { get; set; }
    }
}