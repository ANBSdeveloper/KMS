using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Investments.Dto;
using Cbms.Kms.Application.Investments.Query;
using Cbms.Kms.Application.InvestmentSettings.Commands;
using Cbms.Kms.Application.InvestmentSettings.Dto;
using Cbms.Kms.Application.InvestmentSettings.Query;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [Route("api/v1/investments")]
    public class InvestmentController : AppController
    {
        public InvestmentController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet(Name = "GetInvestment")]
        [Produces(typeof(ApiResultObject<InvestmentDto>))]
        public async Task<InvestmentDto> Get()
        {
            return await Mediator.Send(new InvestmentGet());
        }
    }
}