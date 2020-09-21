using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace TelegramCw.Tools
{
    /// <summary>
    /// Класс для работы с изображениями.
    /// </summary>
    public static class ImagesWorker
    {
        public static string GetScreenshot()
        {
            var resolution = ScreenWorker.GetResolution();
            using var bitmap = new Bitmap((int) resolution.X, (int) resolution.Y);
            using var g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(0, 0, 0, 0,
                    bitmap.Size, CopyPixelOperation.SourceCopy);

            var filePath = GetFilePath();

            var s = File.Create(filePath);
            bitmap.Save(s, ImageFormat.Png);
            s.Close();

            return filePath;
        }

        private static string GetFilePath()
        {
            var path = Assembly.GetEntryAssembly()?.Location;
            
            var parts = path.Split('\\');

            path = string.Empty;
            
            for (int i = 0; i < parts.Length - 1; i++)
            {
                path += parts[i] + "\\";
            }

            path += Infrastructure.DataStore.TMP_IMAGE_FILE_NAME;
            
            return path;
        }
    }
}