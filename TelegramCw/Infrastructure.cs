namespace TorrentBot
{
    /// <summary>
    /// Класс, хранящий в себе постоянные данные, необходимые для работы системы.
    /// </summary>
    public static class Infrastructure
    {
        /// <summary>
        /// Данные, необходимые для подключения
        /// </summary>
        public static class Connection
        {
            /// <summary>
            /// Токен, необходимый для идентификации бота.
            /// </summary>
            public const string CONNECTION_TOKEN = "991120396:AAHnUOcjzCLs4s7V5yIcmdMGnQ-FurXGy60";
        }

        /// <summary>
        /// Данные, необходимые для организации хранения данных.
        /// </summary>
        public static class DataStore
        {
            /// <summary>
            /// Имя файла, хранящего в себе некоторую информацию, необходимую для процесса работы программы.
            /// </summary>
            public const string CONFIG_FILE_NAME = "config.json";
        }
    }
}