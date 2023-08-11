using System;

namespace Cbms.Kms.Application.Integration.Dto
{
    public class PgUserDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
		public string Phone { get; set; }
        public string Email { get; set; }
		public string CategoryCode { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsActive { get; set; }
	}
}
