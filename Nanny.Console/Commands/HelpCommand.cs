using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Nanny.Console.IO;

namespace Nanny.Console.Commands
{
    public class HelpCommand : Command
    {
        private Key _key = new Key("help", "h");
        private IServiceProvider _serviceProvider;

        public HelpCommand(IServiceProvider serviceProvider, IPrinter printer) : base(printer)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Execute()
        {
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
            foreach (var command in list)
            {
                if (command is HelpCommand)
                {
                    sb.AppendLine($"  nanny {_key}      # pass tokens for Jira and Github");
                }
                else
                {
                    sb.AppendLine(command.HelpMessage());
                }
            }
            return sb.ToString();
        }
    }
}