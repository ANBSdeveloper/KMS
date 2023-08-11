﻿using Cbms.Kms.Application.AppSettings.Dto;
using MediatR;
using System.Collections.Generic;

namespace Cbms.Kms.Application.AppSettings.Query
{
    public class GetSalesAppSettingList : IRequest<List<SalesAppSettingDto>>
    {
        public GetSalesAppSettingList()
        {
        }
    }
}