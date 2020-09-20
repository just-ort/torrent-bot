using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;
using System.Timers;
using Telegram.Bot.Types.InputFiles;
using TelegramCw.Tools;

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
                        var text = message.Text;
                        var chatId = message.Chat.Id;
                        switch (text)
                        {
                            case Infrastructure.Commands.GET_PROCESSES:
                                var processes = ProcessesWorker.GetProcesses();
                                await SendStrings(processes, chatId);
                                break;
                            
                            case Infrastructure.Commands.UNLOG:
                                SystemWorker.Unlog();
                                await SendString("Произведен выход из системы!", chatId);
                                break;
                            
                            //Тестовая шляпа для отправки изображений.
                            case Infrastructure.Commands.GET_SCREEN:
                                //Пока работает некорректно.
                                var stream = ImagesWorker.GetScreenshot();
                                await SendImage(stream, chatId);
                                //stream.Close();
                                break;
                            
                            default:
                                await SendString("Неизвестная команда. Попробуйте еще раз.", chatId);
                                break;
                        }
                        
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
            catch (JsonException e)
            {
                Console.WriteLine("Файл данных отсутствует. Все значения будут присвоены по-умолчанию.");
                data = new SerializableData();
            }

            return data;
        }

        /// <summary>
        /// Посылает строку.
        /// </summary>
        /// <param name="messageString">Строка сообщения.</param>
        /// <param name="chatId">Идентификатор чата.</param>
        private async Task SendString(string messageString, long chatId) =>
            await _bot.SendTextMessageAsync(chatId, messageString);

        /// <summary>
        /// Посылает коллекцию строк.
        /// </summary>
        /// <param name="strings">Строки, которые необходимо послать.</param>
        /// <param name="chatId">Идентификатор чата.</param>
        private async Task SendStrings(IEnumerable<string> strings,  long chatId)
        {
            var messageText = string.Empty;

            foreach (var s in strings)
            {
                var tmpMessageText = messageText + s + "\n";;

                //Макс. длина сообщения в ТГ.
                if (tmpMessageText.Length > 4096)
                {
                    await _bot.SendTextMessageAsync(chatId, messageText);
                    messageText = string.Empty;
                }
                
                messageText += s + "\n";
            }

            await _bot.SendTextMessageAsync(chatId, messageText);
        }

        /// <summary>
        /// Посылает изображение.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="chatId">Идентификатор чата.</param>
        private async Task SendImage(MemoryStream stream, long chatId)
        {
            //Пока не работает, говорит, что изображение пустое.
            //Не хочу делать, через сохранение на диск, потому ебусь пока так.
            //await using var fileStream = File.Open(filePath, FileMode.Open);
            var onlineFile = new InputOnlineFile(stream);
            await _bot.SendPhotoAsync(chatId, onlineFile);
        }
    }
}