using AutoMapper;
using Cbms.Kms.Domain.AppSettings;

namespace Cbms.Kms.Application.AppSettings.Dto
{
    [AutoMap(typeof(AppSetting))]
    public class SalesAppSettingDto
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }
}