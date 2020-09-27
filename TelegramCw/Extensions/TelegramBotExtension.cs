using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace TorrentBot.Extensions
{
    public static class TelegramBotExtension
    {
        /// <summary>
        /// Посылает коллекцию строк.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="strings">Строки, которые необходимо послать.</param>
        public static async Task SendStrings(this TelegramBotClient bot, long chatId, IEnumerable<string> strings)
        {
            var messageText = string.Empty;

            foreach (var s in strings)
            {
                var tmpMessageText = messageText + s + "\n";

                //Макс. длина сообщения в ТГ.
                if (tmpMessageText.Length > 4096)
                {
                    await bot.SendTextMessageAsync(chatId, messageText);
                    messageText = string.Empty;
                }

                messageText += s + "\n";
            }

            await bot.SendTextMessageAsync(chatId, messageText);
        }

        /// <summary>
        /// Посылает изображение.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="path">Путь к файлу.</param>
        public static async Task SendImage(this TelegramBotClient bot, long chatId, string path)
        {
            var file = File.Open(path, FileMode.Open);
            var onlineFile = new InputOnlineFile(file);
            await bot.SendPhotoAsync(chatId, onlineFile);
            file.Close();
        }
    }
}