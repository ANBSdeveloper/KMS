using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Helpers
{
    public interface IImageResizer
    {
        Task<string> ResizeBase64Image(string data);

		Task<string> SaveImgFromBase64(string imgObject, string orderNumber, string newImg, string oldImg,
			string imgSavePath, string imgLivePath);
	}
}
