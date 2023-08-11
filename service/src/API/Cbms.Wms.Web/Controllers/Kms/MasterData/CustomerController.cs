using Cbms.Application.Authorization;
using Cbms.Application.Runtime;
using Cbms.AspNetCore.Web.Models;
using Cbms.Authorization;
using Cbms.Kms.Application.Customers.Commands;
using Cbms.Kms.Application.Customers.Dto;
using Cbms.Kms.Application.Customers.Query;
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
    [Route("api/v1/customers")]
    public class CustomerController : AppController
    {
        public CustomerController(ILogger logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        [Produces(typeof(ApiResultObject<CustomerDto>))]
        public async Task<CustomerDto> Get(int id)
        {
            return await Mediator.Send(new CustomerGet(id));
        }

        [HttpGet("{code}/qrdata", Name = "GetCustomerQrData")]
        [Produces(typeof(ApiResultObject<string>))]
        [AllowAnonymous]
        public async Task<string> GetQrData(string code)
        {
            return await Mediator.Send(new CustomerGetQrData(code));
        }

        [HttpGet("codes/{code}", Name = "GetCustomerByCode")]
        [Produces(typeof(ApiResultObject<CustomerDto>))]
        public async Task<CustomerDto> GetByCode(string code)
        {
            return await Mediator.Send(new CustomerGetByCode(code));
        }

        [HttpGet(Name = "GetCustomers")]
        [Produces(typeof(ApiResultObject<PagingResult<CustomerDto>>))]
        public async Task<PagingResult<CustomerDto>> GetList([FromQuery] CustomerGetList query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("getCustomersByStaff", Name = "GetCustomersByStaff")]
        [Produces(typeof(ApiResultObject<PagingResult<CustomerByStaffListDto>>))]
        public async Task<PagingResult<CustomerByStaffListDto>> GetCustomersByStaff([FromQuery] CustomerGetListByStaff query)
        {
            var entity = await Mediator.Send(query);
            return entity;
        }

        [HttpGet("recentSales", Name = "GetRecentSales")]
        [Produces(typeof(ApiResultObject<CustomerRecentSalesDto>))]
        public async Task<CustomerRecentSalesDto> GetCustomerRecentSales([FromQuery] CustomerGetRecentSales query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("register", Name = "RegisterKeyShop")]
        [Produces(typeof(ApiResultObject<object>))]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Customers", "Customers.RegisterKeyShop")]
        public async Task RegisterKeyShop([FromBody] CustomerRegisterKeyShopCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("registerNew", Name = "Register")]
        [Produces(typeof(ApiResultObject<object>))]
        [AllowAnonymous]
        public async Task RegisterNew([FromBody] CustomerRegisterCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("approve", Name = "ApproveKeyShop")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Customers", "Customers.ApproveKeyShop")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task ApproveKeyShop([FromBody] CustomerApproveKeyShopCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("refuse", Name = "RefuseKeyShop")]
        [ClaimRequirementAny(CbmsClaimTypes.Permission, "Customers", "Customers.RefuseKeyShop")]
        [Produces(typeof(ApiResultObject<object>))]
        public async Task RefuseKeyShop([FromBody] CustomerRefuseKeyShopCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("validate", Name = "ValidateActivationKeyShop")]
        [Produces(typeof(ApiResultObject<object>))]
        [AllowAnonymous]
        public async Task ValidateKeyShop([FromBody] CustomerValidateActivationKeyShopCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("activate", Name = "ActivateKeyShop")]
        [Produces(typeof(ApiResultObject<object>))]
        [AllowAnonymous]
        public async Task ActivateKeyShop([FromBody] CustomerActivateKeyShopCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("recovery-password", Name = "ValidateRecoveryPassword")]
        [Produces(typeof(ApiResultObject<object>))]
        [AllowAnonymous]
        public async Task RecoveryPassword([FromBody] CustomerValidateRecoveryPasswordCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("reset-password", Name = "ResetPasswordCustomer")]
        [Produces(typeof(ApiResultObject<object>))]
        [AllowAnonymous]
        public async Task ResetPassword([FromBody] CustomerResetPasswordCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost("check-otp", Name = "CheckCustomerOtp")]
        [Produces(typeof(ApiResultObject<object>))]
        [AllowAnonymous]
        public async Task CheckOtp([FromBody] CustomerCheckOtpCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpGet("approve", Name = "GetCustomerApproveKeyShopList")]
        [Produces(typeof(ApiResultObject<PagingResult<CustomerApproveKeyShopListDto>>))]
        public async Task<PagingResult<CustomerApproveKeyShopListDto>> GetCustomerApproveKeyShopList([FromQuery] CustomerGetListApproveKeyShop query)
        {
            return await Mediator.Send(query);
        }
    }
}