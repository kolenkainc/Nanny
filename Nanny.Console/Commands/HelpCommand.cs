namespace Nanny.Console.Commands
{
    public class HelpCommand : Command
    {
        private string _output =
            "Привет\n" +
            "Как использовать Nanny мы еще не знаем";
        private Key _key = new Key("help", "h");

        public override string Execute()
        {
            return _output;
        }

        public override Key Key()
        {
            return _key;
        }
    }
}