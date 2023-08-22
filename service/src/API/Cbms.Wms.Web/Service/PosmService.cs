using Cbms.Dependency;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Infrastructure;
using Cbms.Kms.Application.Connection;
using Cbms.Authorization;
using Cbms.Kms.Domain.Helpers;
using Cbms.Kms.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using Cbms.Kms.Domain.PosmInvestments;
using IdentityServer4.Extensions;
using System;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Cbms.Kms.Web.Service
{
	public class PosmService
	{
		public IIocResolver IocResolver { get; private set; }	
		private readonly AppDbContext _dbContext;

		private static string LOG_FILE_DIR = AppSettingsConnect.LogsPath + @"/PosmService/";
		private static string LOG_FILE = AppSettingsConnect.LogsPath + @"/PosmService/LogFile_{0}.log";

		public PosmService(IIocResolver iocResolver, AppDbContext dbContext)
		{
			IocResolver = iocResolver;		
			_dbContext = dbContext;
		}

		public async Task<ResponseData> PosmInvestmentItemsUpdate(PosmInvestmentItemsUpdateRequest request)
		{
			ResponseData rsData = new ResponseData();
			try
			{				
				var imageResizer = IocResolver.Resolve<IImageResizer>();

				//Update Deliveries
				var posmInvestmentItemsUpdate = _dbContext.PosmInvestmentItems.FirstOrDefault(p => p.Id == request.id);

				if (posmInvestmentItemsUpdate == null)
				{
					return rsData.ErrorResponse(null, "PosmInvestmentItems not found");
				}
				string imgObject = "PosmInvestmentItems";
				if (!string.IsNullOrEmpty(request.OperationPhoto))
				{
					if (request.PhotoIndex == 1)
					{
						string imgPathPhoto1 = await imageResizer.SaveImgFromBase64(imgObject, request.id.ToString(), request.OperationPhoto, posmInvestmentItemsUpdate.OperationPhoto1, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath);
						posmInvestmentItemsUpdate.OperationPhoto1 = imgPathPhoto1;
					}
					if (request.PhotoIndex == 2)
					{
						string imgPathPhoto2 = await imageResizer.SaveImgFromBase64(imgObject, request.id.ToString(), request.OperationPhoto, posmInvestmentItemsUpdate.OperationPhoto2, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath);
						posmInvestmentItemsUpdate.OperationPhoto2 = imgPathPhoto2;
					}
					if (request.PhotoIndex == 3)
					{
						string imgPathPhoto3 = await imageResizer.SaveImgFromBase64(imgObject, request.id.ToString(), request.OperationPhoto, posmInvestmentItemsUpdate.OperationPhoto3, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath);
						posmInvestmentItemsUpdate.OperationPhoto3 = imgPathPhoto3;
					}
					if (request.PhotoIndex == 4)
					{
						string imgPathPhoto4 = await imageResizer.SaveImgFromBase64(imgObject, request.id.ToString(), request.OperationPhoto, posmInvestmentItemsUpdate.OperationPhoto4, AppSettingsConnect.ImgSavePath, AppSettingsConnect.ImgLivePath);
						posmInvestmentItemsUpdate.OperationPhoto4 = imgPathPhoto4;
					}
				}
				else {
					if (request.PhotoIndex == 1)
					{
						if (posmInvestmentItemsUpdate.OperationPhoto1.Contains("/assets/img_save/"))
						{												
							try
							{
								string fileDir = AppSettingsConnect.ImgSavePath + posmInvestmentItemsUpdate.OperationPhoto1;
								if (File.Exists(fileDir))
								{
									File.Delete(fileDir);
								}
							}
							catch { }
						}
						posmInvestmentItemsUpdate.OperationPhoto1 = "";
					}
					if (request.PhotoIndex == 2)
					{
						if (posmInvestmentItemsUpdate.OperationPhoto2.Contains("/assets/img_save/"))
						{
							try
							{
								string fileDir = AppSettingsConnect.ImgSavePath + posmInvestmentItemsUpdate.OperationPhoto2;
								if (File.Exists(fileDir))
								{
									File.Delete(fileDir);
								}
							}
							catch { }
						}
						posmInvestmentItemsUpdate.OperationPhoto2 = "";
					}
					if (request.PhotoIndex == 3)
					{
						if (posmInvestmentItemsUpdate.OperationPhoto3.Contains("/assets/img_save/"))
						{
							try
							{
								string fileDir = AppSettingsConnect.ImgSavePath + posmInvestmentItemsUpdate.OperationPhoto3;
								if (File.Exists(fileDir))
								{
									File.Delete(fileDir);
								}
							}
							catch { }
						}
						posmInvestmentItemsUpdate.OperationPhoto3 = "";
					}
					if (request.PhotoIndex == 4)
					{
						if (posmInvestmentItemsUpdate.OperationPhoto4.Contains("/assets/img_save/"))
						{
							try
							{
								string fileDir = AppSettingsConnect.ImgSavePath + posmInvestmentItemsUpdate.OperationPhoto4;
								if (File.Exists(fileDir))
								{
									File.Delete(fileDir);
								}
							}
							catch { }
						}
						posmInvestmentItemsUpdate.OperationPhoto4 = "";
					}
				}

				_dbContext.SaveChanges();

				return rsData.SuccessResponse(null, "Update PosmInvestmentItems Success");
			}
			catch (Exception ex)
			{
				//Write log				
				FunctionHelper.WriteLogFile("[Error] ", "PosmInvestmentItemsUpdate Fail, Request Data: " + JsonConvert.SerializeObject(request) + ", Exception:" + ex.ToString(), LOG_FILE_DIR, LOG_FILE, "PosmInvestmentItemsUpdate");
				return rsData.ErrorResponse(null, "Update PosmInvestmentItems Success");
			}
		}
	}
}
