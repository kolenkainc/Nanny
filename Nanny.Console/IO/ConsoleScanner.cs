using Microsoft.Extensions.Logging;

namespace Nanny.Console.IO
{
    public class ConsoleScanner : IScanner
    {
        private ILogger<ConsoleScanner> _log;
        
        public ConsoleScanner(ILogger<ConsoleScanner> log)
        {
            _log = log;
        }

        public string Scan()
        {
            _log.LogInformation("Start scanning");
            return ReadLine();
        }

        private string ReadLine()
        {
            return System.Console.ReadLine();
        }
    }
}
