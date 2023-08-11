using AutoMapper;
using Cbms.Kms.Application.AppSettings.Dto;
using Cbms.Kms.Domain.AppSettings.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cbms.Kms.Application.AppSettings
{
    public class AppSettingMaoProfile : Profile
    {
        public AppSettingMaoProfile()
        {
            CreateMap<UpsertAppSettingDto, AppSettingUpsertAction>();
        }
    }
}
