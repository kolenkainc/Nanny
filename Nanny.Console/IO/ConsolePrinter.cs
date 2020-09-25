using Microsoft.Extensions.Logging;
using Nanny.Console.Commands;

namespace Nanny.Console.IO
{
    public class ConsolePrinter : Printer
    {
        private ILogger<ConsolePrinter> _log;
        
        public ConsolePrinter(ILogger<ConsolePrinter> log)
        {
            _log = log;
        }

        public override void Print(string line)
        {
            _log.LogInformation("Start printing");
            WriteLine(line);
        }

        private void WriteLine(string output)
        {
            System.Console.WriteLine(output);
        }
    }
}