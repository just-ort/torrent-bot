using System;
using System.Management;

namespace TelegramCw
{
    public class ProcessListener
    {
        /// <summary>
        /// Следит за появлением нового процесса.
        /// </summary>
        private ManagementEventWatcher _startWatch;
        
        public ProcessListener()
        {
            _startWatch = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            
            _startWatch.EventArrived += OnProcessStarted;
            _startWatch.Start();
        }

        /// <summary>
        /// Остановить отслеживание запуска процессов.
        /// </summary>
        public void StopListen() => _startWatch.Stop();

        /// <summary>
        /// Обработчик события начала нового процесса.
        /// </summary>
        private void OnProcessStarted(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("Process started: {0}", e.NewEvent.Properties["ProcessName"].Value);
        }
    }
}