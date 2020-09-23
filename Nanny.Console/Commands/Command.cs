namespace Nanny.Console.Commands
{
    public abstract class Command
    {
        public abstract string Execute();
        public abstract Key Key();

        public bool IsSuite(string name)
        {
            return Key().IsSuite(name);
        }
    }
}