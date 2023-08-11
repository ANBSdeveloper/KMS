using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Kms.Application.MaterialTypes.Dto;
using Cbms.Kms.Application.MaterialTypes.Query;
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
    [Route("api/v1/material-types")]
    public class MaterialTypeController : AppController
    {
        public MaterialTypeController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetMaterialType")]
        [Produces(typeof(ApiResultObject<MaterialTypeDto>))]
        public async Task<MaterialTypeDto> Get(int id)
        {
            return await Mediator.Send(new MaterialTypeGet(id));
        }

        [HttpGet(Name = "GetMaterialTypes")]
        [Produces(typeof(ApiResultObject<PagingResult<MaterialTypeDto>>))]
        public async Task<PagingResult<MaterialTypeDto>> GetList([FromQuery] MaterialTypeGetList query)
        {
            return await Mediator.Send(query);
        }
    }
}