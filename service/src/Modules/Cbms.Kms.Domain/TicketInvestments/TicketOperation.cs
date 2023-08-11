using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketOperation : AuditedEntity
    {
        public DateTime OperationDate { get; private set; }
        public string Note { get; private set; }
        public int StockQuantity { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
        public string Photo5 { get; private set; }
        public int TicketInvestmentId { get; private set; }
        public int UpdateUserId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketOperationUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        public async Task UpsertAsync(TicketOperationUpsertAction action)
        {         
            var imageResizer = action.IocResolver.Resolve<IImageResizer>();

            OperationDate = action.OperationDate;
            UpdateUserId = action.UserId;
            StockQuantity = action.StockQuantity;
            Note = action.Note ?? "";
            Photo1 = action.Photo1 != Photo1 ? await imageResizer.ResizeBase64Image(action.Photo1) : Photo1;
            Photo2 = action.Photo2 != Photo2 ? await imageResizer.ResizeBase64Image(action.Photo2) : Photo2;
            Photo3 = action.Photo3 != Photo3 ? await imageResizer.ResizeBase64Image(action.Photo3) : Photo3;
            Photo4 = action.Photo4 != Photo4 ? await imageResizer.ResizeBase64Image(action.Photo4) : Photo4;
            Photo5 = action.Photo5 != Photo5 ? await imageResizer.ResizeBase64Image(action.Photo5) : Photo5;
        }
    }
}