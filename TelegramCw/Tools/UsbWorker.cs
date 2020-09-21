using System.Collections.Generic;
using System.Management;

namespace TelegramCw.Tools
{
    /// <summary>
    /// Класс, отвечающий за работу с USB.
    /// </summary>
    public static class UsbWorker
    {
        /// <summary>
        /// Возвращает список подключенных USB-устройств.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDevices()
        {
            var devicesDescription = new List<string>();
            
            //TODO: потестить на реальной флешке.
            var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub");

            var devices = searcher.Get();

            foreach (var res in devices)
            {
                devicesDescription.Add(
                    $"Id: {res.GetPropertyValue("DeviceID")}  " +
                    $"PnpId: {res.GetPropertyValue("PNPDeviceID")}  " +
                    $"Description: {res.GetPropertyValue("Description")}");
            }
            
            devices.Dispose();
            
            return devicesDescription;
        }
    }
}