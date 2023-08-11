using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketProgressMaterial : AuditedEntity
    {
        public int MaterialId { get; private set; }
        public bool IsReceived { get; private set; }
        public bool IsSentDesign { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
        public string Photo5 { get; private set; }
        public int TicketProgressId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketProgressMaterialUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
            }
        }

        private async Task UpsertAsync(TicketProgressMaterialUpsertAction action)
        {
            MaterialId = action.MaterialId;
            IsReceived = action.IsReceived;
            IsSentDesign = action.IsSentDesign;

            var imageResizer = action.IocResolver.Resolve<IImageResizer>();

            Photo1 = action.Photo1 != Photo1 ? await imageResizer.ResizeBase64Image(action.Photo1) : Photo1;
            Photo2 = action.Photo2 != Photo2 ? await imageResizer.ResizeBase64Image(action.Photo2) : Photo2;
            Photo3 = action.Photo3 != Photo3 ? await imageResizer.ResizeBase64Image(action.Photo3) : Photo3;
            Photo4 = action.Photo4 != Photo4 ? await imageResizer.ResizeBase64Image(action.Photo4) : Photo4;
            Photo5 = action.Photo5 != Photo5 ? await imageResizer.ResizeBase64Image(action.Photo5) : Photo5;
        }
    }
}