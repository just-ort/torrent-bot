using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TelegramCw.Tools
{
    /// <summary>
    /// Класс для работы с функциями системы.
    /// </summary>
    public static class SystemWorker
    {
        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);

        const int WTS_CURRENT_SESSION = -1;
        static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        /// <summary>
        /// Выходит из учетной записи пользователя в системе.
        /// </summary>
        public static void Unlog()
        {
            if (!WTSDisconnectSession(WTS_CURRENT_SERVER_HANDLE,
                WTS_CURRENT_SESSION, false))
            {
                throw new Win32Exception();   
            }
        }
    }
}