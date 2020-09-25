namespace Nanny.Console.IO
{
    public abstract class Printer : IPrinter
    {
        public abstract void Print(string line);
    }
}