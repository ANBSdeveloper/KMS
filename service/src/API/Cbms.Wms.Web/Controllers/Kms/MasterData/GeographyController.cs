using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Kms.Application.Geography.Areas.Dto;
using Cbms.Kms.Application.Geography.Areas.Query;
using Cbms.Kms.Application.Geography.Districts.Dto;
using Cbms.Kms.Application.Geography.Districts.Query;
using Cbms.Kms.Application.Geography.Provinces.Dto;
using Cbms.Kms.Application.Geography.Provinces.Query;
using Cbms.Kms.Application.Geography.Wards.Dto;
using Cbms.Kms.Application.Geography.Wards.Query;
using Cbms.Kms.Application.Geography.Zones.Dto;
using Cbms.Kms.Application.Geography.Zones.Query;
using Cbms.Mediator.Query.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms.MasterData
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/geography")]
    public class GeographyController : AppController
    {
        public GeographyController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("provinces/{id}", Name = "GetProvince")]
        [Produces(typeof(ApiResultObject<ProvinceDto>))]
        public async Task<ProvinceDto> GetProvince(int id)
        {
            return await Mediator.Send(new GetProvince(id));
        }

        [HttpGet("provinces", Name = "GetProvinces")]
        [Produces(typeof(ApiResultObject<PagingResult<ProvinceDto>>))]
        public async Task<PagingResult<ProvinceDto>> GetProvinceList([FromQuery] GetProvinceList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("districts/{id}", Name = "GetDistrict")]
        [Produces(typeof(ApiResultObject<DistrictDto>))]
        public async Task<DistrictDto> GetDistrict(int id)
        {
            return await Mediator.Send(new GetDistrict(id));
        }

        [HttpGet("districts", Name = "GetDistricts")]
        [Produces(typeof(ApiResultObject<PagingResult<DistrictListDto>>))]
        public async Task<PagingResult<DistrictListDto>> GetDistrictList([FromQuery] GetDistrictList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("areas/{id}", Name = "GetArea")]
        [Produces(typeof(ApiResultObject<AreaDto>))]
        public async Task<AreaDto> GetArea(int id)
        {
            return await Mediator.Send(new GetArea(id));
        }

        [HttpGet("areas", Name = "GetAreas")]
        [Produces(typeof(ApiResultObject<PagingResult<AreaDto>>))]
        public async Task<PagingResult<AreaDto>> GetAreaList([FromQuery] GetAreaList query)
        {
            return await Mediator.Send(query);
        }
        [HttpGet("areabyzones", Name = "GetAreaByZones")]
        [Produces(typeof(ApiResultObject<PagingResult<AreaDto>>))]
        public async Task<PagingResult<AreaDto>> GetAreaByZoneList([FromQuery] GetAreaByZoneList query)
        {
            return await Mediator.Send(query);
        }
        [HttpGet("areabyzonenames", Name = "GetAreaByZoneNames")]
        [Produces(typeof(ApiResultObject<PagingResult<AreaByZoneNameListDto>>))]
        public async Task<PagingResult<AreaByZoneNameListDto>> GetAreaByZoneNameList([FromQuery] GetAreaByZoneNameList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("zones/{id}", Name = "GetZone")]
        [Produces(typeof(ApiResultObject<ZoneDto>))]
        public async Task<ZoneDto> GetZone(int id)
        {
            return await Mediator.Send(new GetZone(id));
        }

        [HttpGet("zones", Name = "GetZones")]
        [Produces(typeof(ApiResultObject<PagingResult<ZoneDto>>))]
        public async Task<PagingResult<ZoneDto>> GetZoneList([FromQuery] GetZoneList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("wards/{id}", Name = "GetWard")]
        [Produces(typeof(ApiResultObject<WardDto>))]
        public async Task<WardDto> GetWard(int id)
        {
            return await Mediator.Send(new GetWard(id));
        }

        [HttpGet("wards", Name = "GetWards")]
        [Produces(typeof(ApiResultObject<PagingResult<WardDto>>))]
        public async Task<PagingResult<WardDto>> GetWardList([FromQuery] GetWardList query)
        {
            var entity = await Mediator.Send(query);
            return entity;
        }
        [HttpGet("wardnames", Name = "GetWardNames")]
        [Produces(typeof(ApiResultObject<PagingResult<WardNameListDto>>))]
        public async Task<PagingResult<WardNameListDto>> GetWardNameList([FromQuery] GetWardNameList query)
        {
            var entity = await Mediator.Send(query);
            return entity;
        }
    }
}