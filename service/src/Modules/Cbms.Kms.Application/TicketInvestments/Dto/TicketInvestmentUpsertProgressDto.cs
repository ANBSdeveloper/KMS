using Cbms.Dto;
using System.Collections.Generic;

namespace Cbms.Kms.Application.TicketInvestments.Dto
{
    public class TicketInvestmentUpsertProgressRewardItemDto
    {
        public int Id { get; set; }
        public int RewardItemId { get; set; }
        public bool IsReceived { get; set; }
    }

    public class TicketInvestmentUpsertProgressMaterialDto
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public bool IsReceived { get; set; }
        public bool IsSentDesign { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }
    }
    public class TicketInvestmentUpsertProgressDto : EntityDto
    {
        

        public int ProgressId { get; set; }
        public string DocumentPhoto1 { get; set; }
        public string DocumentPhoto2 { get; set; }
        public string DocumentPhoto3 { get; set; }
        public string DocumentPhoto4 { get; set; }
        public string DocumentPhoto5 { get; set; }
        public string Note { get; set; }
        public List<TicketInvestmentUpsertProgressRewardItemDto> UpsertRewardItems { get; set; }
        public List<TicketInvestmentUpsertProgressMaterialDto> UpsertMaterials { get; set; }

        public TicketInvestmentUpsertProgressDto ()
        {
            UpsertRewardItems = new List<TicketInvestmentUpsertProgressRewardItemDto>();
            UpsertMaterials = new List<TicketInvestmentUpsertProgressMaterialDto>();
        }
    }
}