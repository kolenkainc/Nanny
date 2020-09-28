using Nanny.Console.IO;

namespace Nanny.Console.Commands
{
    public class VersionCommand : Command
    {
        private string _template =
            "Nanny version: {0}";
        private Key _key = new Key("version", "v");

        public VersionCommand(IPrinter printer) : base(printer)
        {
        }

        public override void Execute()
        {
            Printer.Print(string.Format(_template, typeof(VersionCommand).Assembly.GetName().Version));
        }

        public override Key Key()
        {
            return _key;
        }
        
        public override string HelpMessage()
        {
            return $"  nanny {_key}      # get versions of nanny";
        }
    }
}