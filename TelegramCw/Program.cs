using System;
using System.Threading.Tasks;

namespace TorrentBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandHandler = new CommandHandler();
            Console.ReadLine();
            commandHandler.StopReceiving();
        }
    }
}