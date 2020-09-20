using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TelegramCw.Tools
{
    /// <summary>
    /// Класс для работы с изображениями.
    /// </summary>
    public static class ImagesWorker
    {
        public static MemoryStream GetScreenshot()
        {
            var resolution = ScreenWorker.GetResolution();
            using var bitmap = new Bitmap((int) resolution.X, (int) resolution.Y);
            using var g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(0, 0, 0, 0,
                    bitmap.Size, CopyPixelOperation.SourceCopy);
            
            var s = new MemoryStream();
            bitmap.Save(s, ImageFormat.Png);

            return s;
        }
    }
}