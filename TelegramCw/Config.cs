namespace TorrentBot
{
    /// <summary>
    /// Данные, которые необходимо для работы приложения сохранять в Json-файл.
    /// </summary>
    public struct Config
    {
        /// <summary>
        /// Id бота.
        /// </summary>
        public string BotId { get; set; }
        
        /// <summary>
        /// Путь для сохранения файла.
        /// </summary>
        public string SaveFilePath { get; set; }
        
        /// <summary>
        /// Id чата с ботом.
        /// </summary>
        public long ChatId { get; set; }
    }
}