namespace TelegramCw
{
    /// <summary>
    /// Данные, которые необходимо для работы приложения сохранять в Json-файл.
    /// </summary>
    public struct SerializableData
    {
        /// <summary>
        /// Смещение в массиве обновлений бота.
        /// </summary>
        public int updateOffset;
    }
}