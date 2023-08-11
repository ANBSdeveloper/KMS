using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cbms.Kms.Application.Products.Dto
{
    public class QrCodeResultDto
    {
        public string ProductCode { get; set; }
        public string ParentCode { get; set; }
        public int TotalPackage { get; set; }
        public string QrCode { get; set; }
        public List<string> ChildQrCodes { get; set; }
    }
    
    public class QrCodeProductResultV1Dto
    {
        [JsonProperty("DistributionCode")]
        public string ProductCode { get; set; }
        [JsonProperty("ParentCode")]
        public string ParentCode { get; set; }
        [JsonProperty("TotalPackage")]
        public int TotalPackage { get; set; }
        [JsonProperty("Code")]
        public string QrCode { get; set; }
    }
    public class QrCodeParentResultV1Dto
    {
        [JsonProperty("ProductInfo")]
        public QrCodeProductResultV1Dto ProductInfo { get; set; }
        [JsonProperty("ChildCode")]
        public List<string> ChildQrCodes { get; set; }
    }
    #region Product v2
    public class QrCodeResultV2Dto
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        [JsonProperty("Data")]
        public QrCodeDataResultV2Dto Data { get; set; }
    }
    public class QrCodeDataResultV2Dto
    {
        [JsonProperty("Products")]
        public List<QrCodeProductResultV2Dto> Products { get; set; }
    }
    public class QrCodeProductResultV2Dto
    {
        [JsonProperty("Code")]
        public string QrCode { get; set; }
        [JsonProperty("ProductCode")]
        public string ProductCode { get; set; }
        [JsonProperty("ProductUnit")]
        public string ProductUnit { get; set; }
        [JsonProperty("TotalPacket")]
        public int TotalPackage { get; set; }
        [JsonProperty("ProductStatus")]
        public string ProductStatus { get; set; }
    }
    #endregion
    #region BoxV2
    public class QrCodeBoxResultV2Dto
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        [JsonProperty("Data")]
        public QrCodeUnitResultV2Dto Data { get; set; }
    }
    public class QrCodeUnitResultV2Dto
    {
        [JsonProperty("Box")]
        public List<QrCodeBoxParentProductResultV2Dto> Box { get; set; }
    }
    public class QrCodeBoxParentProductResultV2Dto
    {
        [JsonProperty("Code")]
        public string QrCode { get; set; }
        [JsonProperty("ProductCode")]
        public string ProductCode { get; set; }
        [JsonProperty("ProductUnit")]
        public string ProductUnit { get; set; }
        [JsonProperty("TotalPacket")]
        public int TotalPackage { get; set; }
        [JsonProperty("Products")]
        public List<QrCodeBoxProductResultV2Dto> Products { get; set; }
    }
    public class QrCodeBoxProductResultV2Dto
    {
        [JsonProperty("QrCode")]
        public string QrCode { get; set; }
        [JsonProperty("ProductStatus")]
        public string ProductStatus { get; set; }
    }
    #endregion
}
