namespace TelegramCw
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
            public const string CONNECTION_TOKEN = "1245372046:AAEgj96s30uA4ufTgmIZ1cMf3g2OR4WYA7o";

            /// <summary>
            /// Идентификатор тестового чата для тестовых сообщений бота.
            /// </summary>
            public const string TEST_CHAT_ID = "-1001266851208";
        }

        /// <summary>
        /// Данные, необходимые для организации хранения данных.
        /// </summary>
        public static class DataStore
        {
            /// <summary>
            /// Имя файла, хранящего в себе некоторую информацию, необходимую для процесса работы программы.
            /// </summary>
            public const string JSON_FILE_NAME = "data.json";
        }
    }
}