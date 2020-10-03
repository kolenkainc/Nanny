using System;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nanny.Console.IO;

namespace Nanny.Console.Commands
{
    public class HelpCommand : Command
    {
        private Key _key = new Key("help", "h");
        private IServiceProvider _serviceProvider;
        private ILogger<HelpCommand> _logger;

        public HelpCommand(IServiceProvider serviceProvider, IPrinter printer, ILogger<HelpCommand> logger) : base(printer)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public override void Execute()
        {
            var fs = new FileSystem();
            _logger.LogInformation($"Current dir: {fs.CurrentDirectory().FullName}");
            _logger.LogInformation($"Installation dir: {fs.InstallationDirectory().FullName}");
            Printer.Print(HelpMessage());
        }

        public override Key Key()
        {
            return _key;
        }

        public override string HelpMessage()
        {
            CommandList list = _serviceProvider.GetRequiredService<CommandList>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Commands:");
            var maxLengthCommand = list.OrderByDescending(c => c.Key().ToString().Length)
                .First();
            foreach (var command in list)
            {
                if (command is HelpCommand)
                {
                    sb.AppendLine($"{prefix(maxLengthCommand, command)} # describe available commands");
                }
                else
                {
                    sb.AppendLine($"{prefix(maxLengthCommand, command)} # {command.HelpMessage()}");
                }
            }
            return sb.ToString();
        }

        private string prefix(Command first, Command actual)
        {
            int exampleLength = first.ExampleMessage().Length;
            string delta = "";
            for (int i = 0; i < exampleLength - actual.ExampleMessage().Length; i++)
            {
                delta += " ";
            }

            return $"{actual.ExampleMessage()} {delta}";
        }
    }
}