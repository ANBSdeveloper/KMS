namespace Cbms.Kms.Application.Customers.Dto
{
    public class CustomerResetPasswordDto
    {
        public string NewPassword { get; set; }
        public string MobilePhone { get; set; }
        public string OtpCode { get; set; }
    }
}