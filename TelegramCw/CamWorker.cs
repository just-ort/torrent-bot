using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace TelegramCw
{
    public static class CamWorker
    {
        private static VideoCaptureDevice _device;

        private static string _filePath;

        public static string GetCam()
        {
            var WebcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _device = new VideoCaptureDevice(WebcamColl[0].MonikerString);
            _device.Start();
            _device.NewFrame += new NewFrameEventHandler(Device_NewFrame);

            return _filePath;
        }

        private static void Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var bitmap = (Bitmap)eventArgs.Frame.Clone();

            var filePath = GetFilePath();

            var file = File.Open(filePath, FileMode.Create);

            
            bitmap.Save(file, ImageFormat.Png);

            file.Close();
            _device.SignalToStop();

            _filePath = filePath;
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
