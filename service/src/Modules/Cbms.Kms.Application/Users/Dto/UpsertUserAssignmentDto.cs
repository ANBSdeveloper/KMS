using AutoMapper;
using Cbms.Dto;
using Cbms.Kms.Domain.Users;

namespace Cbms.Application.Users.Dto
{
    [AutoMap(typeof(UserAssignment))]
    public class UpsertUserAssignmentDto : AuditedEntityDto
    {
        public int SalesOrgId { get; set; }
    }
}