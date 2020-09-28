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
            list.Sort((command1, command2) => command2.Key().ToString().Length.CompareTo(command1.Key().ToString().Length));
            foreach (var command in list)
            {
                if (command is HelpCommand)
                {
                    sb.AppendLine($"{prefix(list[0], command)} # describe available commands");
                }
                else
                {
                    sb.AppendLine($"{prefix(list[0], command)} # {command.HelpMessage()}");
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