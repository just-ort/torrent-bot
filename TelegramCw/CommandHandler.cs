using System;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;
using System.Timers;

namespace TelegramCw
{
    /// <summary>
    /// Класс, обрабатывающий поступающие через Telegram команды.
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// Собсна, бот.
        /// </summary>
        private TelegramBotClient _bot;

        /// <summary>
        /// Путь к Json-файлу с данными.
        /// </summary>
        private string _dataFilePath;

        /// <summary>
        /// Данные.
        /// </summary>
        private SerializableData _data;
        
        public CommandHandler()
        {
            _bot = new TelegramBotClient(Infrastructure.Connection.CONNECTION_TOKEN);
            TryCreateJson();
            _data = ParseFromJson();
        }

        /// <summary>
        /// Раз в определенный интервал времени создает бекап данных.
        /// </summary>
        /// <param name="interval">Интервал времени в секундах, раз в который создается бекап. </param>
        public void CreateBackup(double interval)
        {
            var timer = new Timer(interval * 1000);

            timer.Elapsed += delegate
            {
                var data = JsonConvert.SerializeObject(_data);
                File.WriteAllText(_dataFilePath, data);
            };

            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        /// <summary>
        /// Отправляет тестовое сообщение в тестовый чат.
        /// </summary>
        public async Task SendTestMessage()
        {
            await _bot.SendTextMessageAsync(Infrastructure.Connection.TEST_CHAT_ID, "Hello, this is test message!");
        }

        /// <summary>
        /// Проверка наличия обновлений в чате.
        /// </summary>
        public async Task CheckForUpdates()
        {
            await _bot.SetWebhookAsync("");

            while (true)
            {
                var updates = await _bot.GetUpdatesAsync(_data.updateOffset);

                for (int i = _data.updateOffset; i < updates.Length; i++)
                {
                    var update = updates[i];
                    
                    var message = update.Message;
                    
                    if (message?.Type == MessageType.Text)
                    {
                        Console.WriteLine(message.Text);    
                    }

                    _data.updateOffset++;
                }
            }
        }

        /// <summary>
        /// Проверяет наличие Json-файла, необходимого для хранения данных и, если его нет, создает его.
        /// </summary>
        private void TryCreateJson()
        {
            var path = Assembly.GetEntryAssembly()?.Location;
            
            var parts = path.Split('\\');

            path = string.Empty;
            
            for (int i = 0; i < parts.Length - 1; i++)
            {
                path += parts[i] + "\\";
            }

            path += Infrastructure.DataStore.JSON_FILE_NAME;

            var isFileExists = File.Exists(path);

            if (!isFileExists)
            {
                File.Create(path);
            }

            _dataFilePath = path;
        }

        /// <summary>
        /// Парсит данные из Json-файла. Если его не существует, получает значения по умолчанию.
        /// </summary>
        private SerializableData ParseFromJson()
        {
            SerializableData data;

            try
            {
                var text = File.ReadAllText(_dataFilePath);
                data = JsonConvert.DeserializeObject<SerializableData>(text);
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine("Файл данных отсутствует. Все значения будут присвоены по-умолчанию.");
                data = new SerializableData();
            }

            return data;
        }
    }
}