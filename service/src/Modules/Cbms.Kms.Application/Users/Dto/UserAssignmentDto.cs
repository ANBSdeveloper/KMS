using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Users;

namespace Cbms.Application.Users.Dto
{
    [AutoMap(typeof(UserAssignment))]
    public class UserAssignmentDto : AuditedEntityDto
    {
        public int UserId { get; set; }
        public int SalesOrgId { get; set; }
        public string SalesOrgName { get; set; }
    }
}