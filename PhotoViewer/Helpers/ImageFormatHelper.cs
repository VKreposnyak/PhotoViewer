using System.IO;
using System.Linq;

namespace PhotoViewer.Helpers
{
    public static class ImageFormatHelper
    {
        public static string[] ImageFormats = { ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ".webp", ".gif", ".png", ".apng", ".bmp", ".dib", ".tiff", ".tif", ".svg", ".svgz", ".ico", ".xbm" };

        public static bool IsImage(string path)
        {
            var extention = Path.GetExtension(path);
            return ImageFormats.Contains(extention);
        }
    }
}