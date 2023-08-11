using Cbms.Dependency;
using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Kms.Application.Integration.Dto;
using Cbms.Kms.Application.Products.Dto;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Integration;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Domain.ProductPoints;
using Cbms.Kms.Domain.ProductPrices;
using Cbms.Kms.Domain.Products;
using Cbms.Localization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Products
{
    public class ProductManager : IProductManager, ITransientDependency
    {
        private readonly IRepository<InvestmentSetting, int> _investmentSettingRepository;
        private readonly IRepository<ProductPrice, int> _productPriceRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<ProductPoint, int> _productPointRepository;
        private readonly IRepository<OrderDetail, int> _orderDetailRepository;
        private readonly ILocalizationManager _localizationManager;
        private readonly IConfiguration _configuration;
        private readonly IAppSettingManager _settingManager;
        private readonly IAppLogger _appLogger;
        private readonly IRewardAppManager _rewardAppManager;

        public ProductManager(
            IRepository<InvestmentSetting, int> investmentSettingRepository,
            IRepository<ProductPoint, int> productPointRepository,
            IRepository<ProductPrice, int> productPriceRepository,
            IRepository<Product, int> productRepository,
            IRepository<OrderDetail, int> orderDetailPointRepository,
            IConfiguration configuration,
            IAppSettingManager settingManager,
            IAppLogger appLogger,
            ILocalizationManager localizationManager,
            IRewardAppManager rewardAppManager)
        {
            _investmentSettingRepository = investmentSettingRepository;
            _productPriceRepository = productPriceRepository;
            _productPointRepository = productPointRepository;
            _orderDetailRepository = orderDetailPointRepository;
            _localizationManager = localizationManager;
            _configuration = configuration;
            _appLogger = appLogger;
            _settingManager = settingManager;
            _productRepository = productRepository;
            _rewardAppManager = rewardAppManager;
        }

        public async Task<bool> CheckSpoonCodeAsync(string spoonCode)
        {
            var localizationSource = _localizationManager.GetDefaultSource();
            if (string.IsNullOrEmpty(spoonCode))
            {
                throw BusinessExceptionBuilder.Create(localizationSource).MessageCode("Product.SpoonCodeInvalid").Build();
            }
            var existsSpoon = await _orderDetailRepository.FirstOrDefaultAsync(p => p.SpoonCode.ToUpper() == spoonCode.ToUpper());
            if (existsSpoon != null)
            {
                return false;
            }

            bool spoonSampleMode = await _settingManager.IsEnableAsync("SPOON_SAMPLE_MODE");
            if (spoonSampleMode) return true;

            var spoonCodeResult = await FetchSpoonResultFromRA(spoonCode);
            if (!spoonCodeResult)
            {
                throw BusinessExceptionBuilder.Create(localizationSource).MessageCode("Product.SpoonCodeInvalid").Build();
            }
            return true;
        }

        private async Task<bool> FetchSpoonResultFromRA(string spoonCode)
        {
            var result = (await _rewardAppManager.FetchSpoon(spoonCode)) as RewardAppResultDto<SpoonCodeResultDto>;
            if (result.Meta.Error != null || result.Response.Status != "UNUSED")
            {
                return false;
            }

            return true;
        }

        private async Task<QrCodeResultDto> GetNewQrCodeAsync(string qrCode)
        {
            string url = _configuration["QrCode:NewUrl"];
          
            try
            {
                var body = "{\"QrCode\":\"" + qrCode + "\"}";
                string funcation = "GetQrInfo";
                string partner = _configuration["QrCode:Partner"];
                string privateKey = _configuration["QrCode:PrivateKey"];
                string data = @$"{privateKey}|{partner}|{funcation}|{body}";

                string checkSum = SyncSHA(data, privateKey);


                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("function", funcation);
                request.AddHeader("check_sum", checkSum);
                request.AddHeader("sign_type", "SHA256");
                request.AddHeader("encoding", "UTF-8");
                request.AddHeader("partner", partner);
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string reponseContent = response.Content.Replace("\\", "").Replace("\"{","{").Replace("}\"", "}");
                if (qrCode.EndsWith("-A")) {
                    var result = JsonConvert.DeserializeObject<QrCodeBoxResultV2Dto>(reponseContent);
                    if (result.ErrorCode == 0 && result.Data?.Box.Count > 0)
                    {
                        return new QrCodeResultDto()
                        {
                            ParentCode = result.Data.Box[0].ProductCode,
                            ChildQrCodes = result.Data.Box[0].Products.Select(p=>p.QrCode).ToList(),
                            ProductCode = result.Data.Box[0].ProductCode,
                            QrCode = result.Data.Box[0].QrCode,
                            TotalPackage = result.Data.Box[0].TotalPackage,
                        };
                    }
                } 
                else
                {
                    var result = JsonConvert.DeserializeObject<QrCodeResultV2Dto>(reponseContent);
                    if (result.ErrorCode == 0 && result.Data?.Products.Count > 0)
                    {
                        return new QrCodeResultDto()
                        {
                            ParentCode = "",
                            ChildQrCodes = new List<string>(),
                            ProductCode = result.Data.Products[0].ProductCode,
                            QrCode = result.Data.Products[0].QrCode,
                            TotalPackage = result.Data.Products[0].TotalPackage,
                        };
                    }
                }
            }
            catch (Exception ex)
            { 
                await _appLogger.LogInfoAsync("CHECK_QRCODE", new { qrCode, Error = ex });
            }
            return null;

        }

        private string SyncSHA(string data, string key, int algo = 256,string encode = "UTF-8" )
        {
            if (string.IsNullOrEmpty(encode))
            {
                encode = "UTF-8";
            }

            var encoding = Encoding.GetEncoding(encode);

            byte[] buffKey = encoding.GetBytes(key);
            byte[] buffData = encoding.GetBytes(data);

            byte[] hashMessage = null;

            switch(algo)
            {
                case 1:
                    using (var hmacSHA = new HMACSHA1(buffKey))
                        hashMessage = hmacSHA.ComputeHash(buffData);
                    break;
                case 384:
                    using (var hmacSHA = new HMACSHA384(buffKey))
                        hashMessage = hmacSHA.ComputeHash(buffData);
                    break;
                case 512:
                    using (var hmacSHA = new HMACSHA512(buffKey))
                        hashMessage = hmacSHA.ComputeHash(buffData);
                            break;
                case 256:
                default:
                    using (var hmacSHA = new HMACSHA256(buffKey))
                        hashMessage = hmacSHA.ComputeHash(buffData);
                    break;
            }

            var hex = BitConverter.ToString(hashMessage);
            return hex.Replace("-", "").ToLower();
        }

        private async Task<QrCodeResultDto> GetQrCodeAsync(string qrCode)
        {
            bool useOldApi = string.IsNullOrEmpty(_configuration["QrCode:UseOldApi"]) ? false : Convert.ToBoolean(_configuration["QrCode:UseOldApi"]);

            QrCodeResultDto result = null;

            if (useOldApi)
            {
                result = await GetOldQrCodeAsync(qrCode);
            }
            else
            {
                result = await GetNewQrCodeAsync(qrCode);
            }

            return result;
        }

        private async Task<QrCodeResultDto> GetOldQrCodeAsync(string qrCode)
        {
            string url = _configuration["QrCode:Url"];
            try
            {
                await _appLogger.LogInfoAsync("CHECK_QRCODE", new { qrCode });
                var client = new RestClient(url);
                client.UseNewtonsoftJson();
                var request = new RestRequest($"/DesktopModules/WebServicesProduct/api/Product/GetProductByCode/{qrCode}/21.0228424,105.9365616", DataFormat.Json);
                var rawResult = client.Get(request);

                var parentResultDto = JsonConvert.DeserializeObject<QrCodeParentResultV1Dto>(rawResult.Content);
                if (parentResultDto.ProductInfo != null)
                {
                    return new QrCodeResultDto()
                    {
                        ParentCode = parentResultDto.ProductInfo.ParentCode,
                        ChildQrCodes = parentResultDto.ChildQrCodes,
                        ProductCode = parentResultDto.ProductInfo.ProductCode,
                        QrCode = parentResultDto.ProductInfo.QrCode,
                        TotalPackage = parentResultDto.ProductInfo.TotalPackage,
                    };
                }
                var productResultDto = JsonConvert.DeserializeObject<QrCodeProductResultV1Dto>(rawResult.Content);
                return new QrCodeResultDto()
                {
                    ParentCode = productResultDto.ParentCode,
                    ChildQrCodes = new List<string>(),
                    ProductCode = productResultDto.ProductCode,
                    QrCode = productResultDto.QrCode,
                    TotalPackage = productResultDto.TotalPackage,
                };
            }
            catch (Exception ex)
            {
                await _appLogger.LogInfoAsync("CHECK_QRCODE", new { qrCode, Error = ex });
            }
            return null;
        }

        public async Task<ProductInfo> CheckAndGetInfoByQrCodeAsync(int branchId, string qrCode, bool smallUnitRequire)
        {
            var localizationSource = _localizationManager.GetDefaultSource();

            if (string.IsNullOrEmpty(qrCode))
            {
                throw BusinessExceptionBuilder.Create(localizationSource).MessageCode("Product.QrCodeInvalid").Build();
            }

            var orderDetail = _orderDetailRepository.GetAll().Where(p => p.QrCode == qrCode && p.SpoonCode != "").FirstOrDefault();
            if (orderDetail != null)
            {
                var hotline = await _settingManager.GetAsync("HOTLINE_SHOP");
                throw BusinessExceptionBuilder.Create(localizationSource).MessageCode("Product.QrCodeIsUsed", qrCode, hotline).Build();
            }

            var qrCodeResult = await GetQrCodeAsync(qrCode);
            if (qrCodeResult == null || string.IsNullOrEmpty(qrCodeResult.ProductCode))
            {
                var hotline = await _settingManager.GetAsync("HOTLINE_SHOP");
                throw BusinessExceptionBuilder.Create(localizationSource).MessageCode("Product.NotFoundByQrCode", hotline).Build();
            }

            var product = await _productRepository.FirstOrDefaultAsync(p => p.Code == qrCodeResult.ProductCode);
            if (product == null)
            {
                await _appLogger.LogInfoAsync("CHECK_QRCODE", new { qrCode, Error = $"Product {qrCodeResult.ProductCode} not found in KMS" });
                throw BusinessExceptionBuilder.Create(localizationSource).MessageCode("Product.QrCodeInvalid").Build();
            }

            if (smallUnitRequire && !string.IsNullOrEmpty(qrCodeResult.ParentCode))
            {
                await _appLogger.LogInfoAsync("CHECK_QRCODE", new { qrCode, Error = $"Product {qrCodeResult.ProductCode} is only support small unit in this function" });
                throw BusinessExceptionBuilder.Create(localizationSource).MessageCode("Product.QrCodeSmallUnitNotSupport").Build();
            }

            var setting = _investmentSettingRepository.GetAll().FirstOrDefault();
            //TODO Check Setting
            //Product.QrCodeInvalidBranch

            var point = await GetPointsAsync(product.Id);

            return new ProductInfo(qrCodeResult.ParentCode, qrCode, product.Id, product.Code, product.Name, product.Unit, qrCodeResult.TotalPackage, point, qrCodeResult.ChildQrCodes);
        }

        public async Task<decimal> GetPointsAsync(int productId)
        {
            var date = DateTime.Now.Date;
            var productPoint = _productPointRepository
                .GetAll()
                .Where(p =>
                    p.ProductId == productId &&
                    p.FromDate <= date &&
                    p.ToDate >= date && p.IsActive
                )
                .OrderByDescending(p => p.FromDate)
                .FirstOrDefault();

            return productPoint != null ? productPoint.Points : 0;
        }

        public async Task<decimal> GetPriceAsync(int productId)
        {
            var date = DateTime.Now.Date;
            var productPrice = _productPriceRepository.GetAll().Where(p =>
                p.ProductId == productId && p.FromDate <= date).OrderByDescending(p => p.FromDate).FirstOrDefault();

            return productPrice != null ? productPrice.Price : 0;
        }
    }
}