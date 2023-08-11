using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Vendors.Commands;
using Cbms.Kms.Application.Vendors.Dto;
using Cbms.Kms.Application.Vendors.Query;
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
    [Route("api/v1/Vendors")]
    public class VendorController : AppController
    {
        public VendorController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetVendor")]
        [Produces(typeof(ApiResultObject<VendorDto>))]
        public async Task<VendorDto> Get(int id)
        {
            return await Mediator.Send(new VendorGet(id));
        }

        [HttpGet(Name = "GetVendors")]
        [Produces(typeof(ApiResultObject<PagingResult<VendorListDto>>))]
        public async Task<PagingResult<VendorListDto>> GetList([FromQuery] VendorGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost(Name = "CreateVendor")]
        [Produces(typeof(ApiResultObject<VendorDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Vendors")]
        public async Task<VendorDto> Create([FromBody] VendorUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command);
            return entityDto;
        }

        [HttpDelete("{id}", Name = "DeleteVendor")]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Vendors")]
        public async Task Delete(int id)
        {
            await Mediator.Send(new VendorDeleteCommand(id));
        }

        [HttpPut("{id}", Name = "UpdateVendor")]
        [Produces(typeof(ApiResultObject<VendorDto>))]
        [ClaimRequirement(CbmsClaimTypes.Permission, "Vendors")]
        public async Task<VendorDto> Update([FromRoute] int id, [FromBody] VendorUpsertCommand command)
        {
            var entityDto = await Mediator.Send(command.ValidateId(id));
            return entityDto;
        }
    }
}