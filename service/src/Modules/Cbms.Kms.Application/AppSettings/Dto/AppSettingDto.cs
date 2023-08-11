using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.AppSettings;

namespace Cbms.Kms.Application.AppSettings.Dto
{
    [AutoMap(typeof(AppSetting))]
    public class AppSettingDto : AuditedEntityDto
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}