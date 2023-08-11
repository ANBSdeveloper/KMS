using System;

namespace Cbms.Application.Users.Dto
{
    public class UpdateProfileDto
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public UpdateProfileDto()
        {
        }
    }
}