using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TorrentBot.Tools;
using File = System.IO.File;

namespace TorrentBot
{
    /// <summary>
    /// Класс, обрабатывающий поступающие через Telegram команды.
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// Собсна, бот.
        /// </summary>
        private readonly TelegramBotClient _bot;

        /// <summary>
        /// Конфиг-данные.
        /// </summary>
        private Config _config;

        public CommandHandler()
        {
            _config = DataSerializer.Deserialize<Config>();

            if (_config.BotId == null)
            {
                Console.Write("Введите id бота: ");
                _config.BotId = Console.ReadLine();
                
                Console.Write("Введите путь для сохранения торрент-файлов: ");
                _config.SaveFilePath = Console.ReadLine();
                
                DataSerializer.Serialize(_config);
            }

            _bot = new TelegramBotClient(_config.BotId);
            _bot.OnUpdate += OnUpdate;
            _bot.StartReceiving();
            
            Console.WriteLine("Bot start listening...");
        }

        public void StopReceiving() => _bot.StopReceiving();
        
        /// <summary>
        /// Обработчик события обновления чата.
        /// </summary>
        private async void OnUpdate(object? sender, UpdateEventArgs e)
        {
            var message = e.Update.Message;
            var chatId = message.Chat.Id;

            if (_config.ChatId == null || _config.ChatId == 0)
            {
                _config.ChatId = chatId;
                DataSerializer.Serialize(_config);
            }

            switch (message.Type)
            {
                case MessageType.Document:
                    var doc = message.Document;
                    var result = await SaveFile(doc);

                    var text = result
                        ? $"Файл {doc.FileName} успешно загружен и сохранен."
                        : $"Произошла ошибка. Проверьте сервер или попробуйте еще раз.";
                    
                    await _bot.SendTextMessageAsync(chatId, text);
                    break;
                
                case MessageType.Text:
                    await _bot.SendTextMessageAsync(chatId, "Обработка ссылок находится в разработке...");
                    break;
                
                default:
                    await _bot.SendTextMessageAsync(chatId, "Вы можете отправлять боту только документы или ссылки.");
                    break;
            }
        }

        /// <summary>
        /// Загружает файл и сохраняет его на диск.
        /// </summary>
        /// <param name="doc">Загружаемый документ.</param>
        private async Task<bool> SaveFile(Document doc)
        {
            var id = doc.FileId;
            var filePath = Path.Combine(_config.SaveFilePath, doc.FileName);
            var file = File.Create(filePath);

            try
            {
                await _bot.GetInfoAndDownloadFileAsync(id, file);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }

            finally
            {
                file.Close();   
            }
        }
    }
}