using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.PosmPrices.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.PosmPrices
{
    public class PosmPriceHeader : AuditedAggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public bool IsActive { get; private set; }
        public IReadOnlyCollection<PosmPriceDetail> PosmPriceDetails => _posmPriceDetails;        
        public List<PosmPriceDetail> _posmPriceDetails = new List<PosmPriceDetail>();        

        private PosmPriceHeader()
        {
        }

        public static PosmPriceHeader Create()
        {
            return new PosmPriceHeader();
        }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case PosmPriceUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(PosmPriceUpsertAction action)
        {
            Code = action.Code;
            Name = action.Name;
            FromDate = action.FromDate;
            ToDate = action.ToDate;
            IsActive = action.IsActive;
            foreach (var id in action.DeleteItems)
            {
                var posmPriceDetail = _posmPriceDetails.FirstOrDefault(p => p.Id == id);
                if (posmPriceDetail != null)
                {
                    _posmPriceDetails.Remove(posmPriceDetail);
                }
            }

            foreach (var item in action.UpsertDetails)
            {
                var detail = _posmPriceDetails.FirstOrDefault(p => p.PosmItemId == item.PosmItemId);
                if (detail == null)
                {
                    detail = PosmPriceDetail.Create();
                    _posmPriceDetails.Add(detail);
                }
                

                await detail.ApplyActionAsync(item);
            }
        }
    }
}