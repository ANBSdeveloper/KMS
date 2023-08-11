using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Helpers
{
    public interface IImageResizer
    {
        Task<string> ResizeBase64Image(string data);
    }
}
