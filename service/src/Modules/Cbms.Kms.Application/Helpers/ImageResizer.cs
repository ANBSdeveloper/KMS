using Cbms.Dependency;
using Cbms.Kms.Domain.Helpers;
using ImageMagick;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace Cbms.Kms.Application.Helpers
{
	public class ImageResizer : IImageResizer, ISingletonDependency
	{
		public async Task<string> ResizeBase64Image(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return data;
			}
			var base64 = Regex.Replace(data, @"^.+?(;base64),", string.Empty);
			byte[] input = Convert.FromBase64String(base64);
			const int size = 1024;
			const int quality = 100;

			using (var image = new MagickImage(input))
			{
				image.Resize(size, size);
				image.Strip();
				image.Quality = quality;
				image.Format = MagickFormat.Jpeg;
				return image.ToBase64();
			}
		}

		public async Task<string> SaveImgFromBase64(string imgObject, string orderNumber, string newImg, string oldImg,
			string imgSavePath, string imgLivePath)
		{
			if(imgSavePath == "" || imgLivePath == "")
			{
				return oldImg;
			}
			string oldImgPath = imgSavePath + oldImg;			
			string oldImgBase64 = "";

			if (File.Exists(oldImgPath))
			{
				byte[] imageArray = System.IO.File.ReadAllBytes(oldImgPath);
				oldImgBase64 = Convert.ToBase64String(imageArray);
			}

			if (!string.IsNullOrEmpty(newImg) && Regex.Replace(newImg, @"^.+?(;base64),", string.Empty) != oldImgBase64)
			{
				var imgeBase64Resize = await ResizeBase64Image(newImg);

				//Save Path process
				string imgSavePathProcess = imgSavePath + imgLivePath + "/" + imgObject + "/" + DateTime.Now.ToString("ddMMyyyy");
				if (!Directory.Exists(imgSavePathProcess))
				{
					Directory.CreateDirectory(imgSavePathProcess);
				}

				//Delete old Img
				//try
				//{
				//	if (File.Exists(oldImgPath))
				//	{
				//		File.Delete(oldImgPath);
				//	}
				//}
				//catch { }

				string imgFileName = imgObject + "1_" + orderNumber + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmssfff") + ".jpg";
				string imgLivePathProcess = imgLivePath + "/" + imgObject + "/" + DateTime.Now.ToString("ddMMyyyy") + "/" + imgFileName;

				//Save Img
				var bytes = Convert.FromBase64String(imgeBase64Resize);
				using (var imageFile = new FileStream(Path.Combine(imgSavePathProcess, imgFileName), FileMode.Create))
				{
					imageFile.Write(bytes, 0, bytes.Length);
					imageFile.Flush();
				}

				return imgLivePathProcess;
			}
			else
			{
				if (string.IsNullOrEmpty(newImg))
				{
					return null;
				}
				else
				{
					return oldImg;
				}
			}

		}
	}
}