using System.Threading.Tasks;
using Telegram.Bot;
using TorrentBot;
using TorrentBot.Tools;

namespace QBitTorrentDownloadedHandler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = DataSerializer.Deserialize<Config>();
            var name = args[0];
            var bot = new TelegramBotClient(config.BotId);

            await bot.SendTextMessageAsync(
                config.ChatId,
                $"Файл {name} успешно загружен!");
        }
    }
}