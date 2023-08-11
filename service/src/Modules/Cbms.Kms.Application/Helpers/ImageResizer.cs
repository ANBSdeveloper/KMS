using Cbms.Dependency;
using Cbms.Kms.Domain.Helpers;
using ImageMagick;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Helpers
{
    public class ImageResizer: IImageResizer, ISingletonDependency
    {
        public async Task<string> ResizeBase64Image(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            var base64 = Regex.Replace(data, @"^.+?(;base64),", string.Empty);
            byte[] input = Convert.FromBase64String(base64);
            const int size = 800;
            const int quality = 75;

            using (var image = new MagickImage(input))
            {
                image.Resize(size, size);
                image.Strip();
                image.Quality = quality;
                image.Format = MagickFormat.Jpeg;
                return image.ToBase64();
            }
        }
    }
}