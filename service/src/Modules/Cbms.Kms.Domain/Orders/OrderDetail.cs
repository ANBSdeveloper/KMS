using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Orders.Actions;
using Cbms.Kms.Domain.Products;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Orders
{
    public class OrderDetail : AuditedEntity
    {
        public int ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string QrCode { get; private set; }
        public string SpoonCode { get; private set; }
        public string ProductName { get; private set; }
        public string UnitName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal LineAmount { get; private set; }
        public string Api { get; private set; }
        public decimal Points { get; private set; }
        public decimal AvailablePoints { get; private set; }
        public decimal UsedPoints { get; private set; }
        public bool UsedForTicket { get; private set; }
        public int OrderId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case OrderDetailCreateAction upsertAction:
                    await CreateAsync(upsertAction);
                    break;
                case OrderDetailUsePointAction useAction:
                    await UseAsync(useAction);
                    break;
            }
        }

        public async Task CreateAsync(OrderDetailCreateAction action)
        {

            if (string.IsNullOrEmpty(action.QrCode))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.DetailQrCodeRequired", action.CompareProductCode).Build();
            }

           

            var productManager = action.IocResolver.Resolve<IProductManager>();

            var productInfo = await productManager.CheckAndGetInfoByQrCodeAsync(action.BranchId, action.QrCode, true);

            // !IsNullOrEmpty for update spoon from reward app
            if (!string.IsNullOrEmpty(action.CompareProductCode) && productInfo.ProductCode != action.CompareProductCode)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.DetailProductNotMatchQrCode", action.QrCode, action.CompareProductCode).Build();
            }

            if (string.IsNullOrEmpty(action.SpoonCode))
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.DetailSpoonRequired", productInfo.ProductCode).Build();
            }

            ProductCode = productInfo.ProductCode;
            ProductId = productInfo.ProductId;
            UnitName = productInfo.Unit;
            ProductName = productInfo.Name;

            UsedForTicket = false;
            UnitPrice = await productManager.GetPriceAsync(ProductId);
            Quantity = 1;
            LineAmount = Quantity * UnitPrice;


            QrCode = action.QrCode;

            if (action.CheckSpoon)
            {
                var isSpoonValid = await productManager.CheckSpoonCodeAsync(action.SpoonCode);
                if (!isSpoonValid)
                {
                    throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.DetailSpoonInValid", action.SpoonCode, productInfo.ProductCode).Build();
                }
            }
           
            SpoonCode = action.SpoonCode;
            Points = action.UpdatePoint ? await productManager.GetPointsAsync(ProductId) : 0;
            AvailablePoints = Points;
            Api = action.Api;
          
    
        }
        public async Task UseAsync(OrderDetailUsePointAction action)
        {
            UsedPoints += action.UsePoints;
            AvailablePoints -= action.UsePoints;
            UsedForTicket = true;
            if (AvailablePoints < 0)
            {
                throw BusinessExceptionBuilder.Create(action.LocalizationSource).MessageCode("Order.PointsOverLimit").Build();
            }
        }

    }
}