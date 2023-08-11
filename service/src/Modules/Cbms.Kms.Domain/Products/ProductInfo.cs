using System.Collections.Generic;

namespace Cbms.Kms.Domain.Products
{
    public class ProductInfo
    {
        public int ProductId { get; private set; }
        public string QrCode { get; private set; }
        public string ParentCode { get; private set; }
        public string ProductCode { get; private set; }
        public string Name { get; private set; }
        public string Unit { get; private set; }
        public int Quantity { get; private set; }
        public decimal Point { get; private set; }
        public List<string> ChildQrCodes { get; private set; }

        public ProductInfo(string parentCode, string qrCode, int productId, string productCode, string name, string unit, int quantity, decimal point, List<string> childQrCodes)
        {
            ParentCode = parentCode;
            QrCode = qrCode;
            ProductCode = productCode;
            ProductId = productId;
            Name = name;
            Unit = unit;
            Quantity = quantity;
            Point = point;
            ChildQrCodes = childQrCodes;
        }

    }
}
