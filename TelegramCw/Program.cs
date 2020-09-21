using System;
using System.Threading.Tasks;

namespace TelegramCw
{
    class Program
    {
        static async Task Main()
        {
            var commandHandler = new CommandHandler();
            var processListener = new ProcessListener();

            Console.ReadLine();
            commandHandler.StopReceiving();
            processListener.StopListen();
        }
    }
}