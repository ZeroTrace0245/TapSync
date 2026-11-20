using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;

namespace TapSync.Services
{
    public static class ImageLoader
    {
        /// <summary>
        /// Loads a picture from disk through a stream, which works for both packaged and
        /// unpackaged runs. Returns null if the file is missing or not a readable image.
        /// </summary>
        public static async Task<BitmapImage?> LoadAsync(string? path, int decodePixelWidth = 0)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;

            try
            {
                var bitmap = new BitmapImage();
                if (decodePixelWidth > 0)
                {
                    bitmap.DecodePixelWidth = decodePixelWidth;
                }

                using var stream = File.OpenRead(path);
                await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                return bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Could not load image '{path}': {ex.Message}");
                return null;
            }
        }
    }
}
