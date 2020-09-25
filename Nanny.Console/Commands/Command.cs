using Nanny.Console.IO;

namespace Nanny.Console.Commands
{
    public abstract class Command
    {
        protected IPrinter Printer; 
        
        public Command(IPrinter printer)
        {
            Printer = printer;
        }
        public abstract void Execute();
        public abstract Key Key();

        public bool IsSuite(string name)
        {
            return Key().IsSuite(name);
        }
    }
}