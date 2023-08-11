using Cbms.Application.Report;
using Cbms.Application.Runtime;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Web.Controllers.Kms
{
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "api")]
    [ApiController]
    public class ReportController : AppController
    {

        public ReportController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }
        [HttpGet]
        public async Task<object> GetData(string @params)
        {
            var base64EncodedBytes = Convert.FromBase64String(@params);
            var result = await Mediator.Send(new GetReportData(System.Text.Encoding.UTF8.GetString(base64EncodedBytes)));
            return result;
        }
    }
}