using Cbms.Domain.Entities;
using Cbms.Domain.Entities.Auditing;
using Cbms.Kms.Domain.Connection;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Domain.TicketInvestments.Actions;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.TicketInvestments
{
    public class TicketAcceptance : AuditedEntity
    {
        public DateTime AcceptanceDate { get; private set; }
        public string Photo1 { get; private set; }
        public string Photo2 { get; private set; }
        public string Photo3 { get; private set; }
        public string Photo4 { get; private set; }
        public string Photo5 { get; private set; }
        public string Note { get; private set; }
        public decimal? RemarkOfSales { get; private set; }
        public decimal? RemarkOfCompany { get; private set; }
        public decimal? RemarkOfCustomerDevelopement { get; private set; }
        public int UpdateUserId { get; private set; }
        public int TicketInvestmentId { get; private set; }

        public async override Task ApplyActionAsync(IEntityAction action)
        {
            switch (action)
            {
                case TicketAcceptanceUpsertAction upsertAction:
                    await UpsertAsync(upsertAction);
                    break;
                case TicketAcceptanceSalesRemarkAction remarkAction:
                    await SalesRemarkAsync(remarkAction);
                    break;
                case TicketAcceptanceCompanyRemarkAction companyRemarkAction:
                    await CompanyRemarkAsync(companyRemarkAction);
                    break;
                case TicketAcceptanceCustomerDevelopmentRemarkAction customerDevelopmentRemarkAction:
                    await CustomerDevelopmentRemarkAsync(customerDevelopmentRemarkAction);
                    break;
            }
        }

        public async Task UpsertAsync(TicketAcceptanceUpsertAction action)
        {
            var imageResizer = action.IocResolver.Resolve<IImageResizer>();
            AcceptanceDate = action.AcceptanceDate.ToLocalTime().Date;
            UpdateUserId = action.UserId;
            Note = action.Note ?? "";

            //Photo1 = action.Photo1 != Photo1 ? await imageResizer.ResizeBase64Image(action.Photo1) : Photo1;
            //Photo2 = action.Photo2 != Photo2 ? await imageResizer.ResizeBase64Image(action.Photo2) : Photo2;
            //Photo3 = action.Photo3 != Photo3 ? await imageResizer.ResizeBase64Image(action.Photo3) : Photo3;
            //Photo4 = action.Photo4 != Photo4 ? await imageResizer.ResizeBase64Image(action.Photo4) : Photo4;
            //Photo5 = action.Photo5 != Photo5 ? await imageResizer.ResizeBase64Image(action.Photo5) : Photo5;

			Photo1 = !string.IsNullOrEmpty(action.Photo1) ? await imageResizer.SaveImgFromBase64("TicketAcceptance", "", action.Photo1, Photo1, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath) : "";
			Photo2 = !string.IsNullOrEmpty(action.Photo2) ? await imageResizer.SaveImgFromBase64("TicketAcceptance", "", action.Photo2, Photo2, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath) : "";
			Photo3 = !string.IsNullOrEmpty(action.Photo3) ? await imageResizer.SaveImgFromBase64("TicketAcceptance", "", action.Photo3, Photo3, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath) : "";
			Photo4 = !string.IsNullOrEmpty(action.Photo4) ? await imageResizer.SaveImgFromBase64("TicketAcceptance", "", action.Photo4, Photo4, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath) : "";
			Photo5 = !string.IsNullOrEmpty(action.Photo5) ? await imageResizer.SaveImgFromBase64("TicketAcceptance", "", action.Photo5, Photo5, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath) : "";
		}

        public async Task SalesRemarkAsync(TicketAcceptanceSalesRemarkAction action)
        {
            RemarkOfSales = action.Remark;
        }

        public async Task CompanyRemarkAsync(TicketAcceptanceCompanyRemarkAction action)
        {
            RemarkOfCompany = action.Remark;
        }

        public async Task CustomerDevelopmentRemarkAsync(TicketAcceptanceCustomerDevelopmentRemarkAction action)
        {
            RemarkOfCustomerDevelopement = action.Remark;
        }
    }
}