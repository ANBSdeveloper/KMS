using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Products
{
    public interface IProductManager
    {
        Task<ProductInfo> CheckAndGetInfoByQrCodeAsync(int branchId, string qrCode, bool smallUnitRequire);
        Task<bool> CheckSpoonCodeAsync(string spoonCode);
        Task<decimal> GetPriceAsync(int productId);
        Task<decimal> GetPointsAsync(int productId);
    }
}
