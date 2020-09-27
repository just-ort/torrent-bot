using System;
using System.Diagnostics;
using System.Linq;

namespace TorrentBot.Tools
{
    /// <summary>
    /// Класс, выполняющий работу с процессами.
    /// </summary>
    public static class ProcessesWorker
    {
        /// <summary>
        /// Возвращает информацию о запущенных процессах.
        /// </summary>
        public static string[] GetProcesses()
        {
            var processes = Process.GetProcesses().ToList();

            var info = processes.Select(p =>
                $"Name: {p.ProcessName}  ID: {p.Id}  RAM: {p.PagedMemorySize64 / 1048576}MB").ToArray();

            return info;
        }

        /// <summary>
        /// Блокирует указанный процесс.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool TryBlockProcesses()
        {
            throw new NotImplementedException("В процессе");
        }

        /// <summary>
        /// Разблокирует указанный процесс.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool TryUnblockProcess()
        {
            throw new NotImplementedException("В процессе");
        }
    }
}