namespace Cbms.Kms.Application.Notifications.Dto
{
    internal class SmsResultDto
    {
        public string msgid { get; set; }
        public string error { get; set; }
        public string error_code { get; set; }
        public string log { get; set; }
        public string carrier { get; set; }
    }
}