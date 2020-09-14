using System.Threading.Tasks;

namespace TelegramCw
{
    class Program
    {
        
        static async Task Main()
        {
            var commandHandler = new CommandHandler();
            
            //TODO: поменять время.
            commandHandler.CreateBackup(30);
            
            await commandHandler.CheckForUpdates();
        }
    }
}