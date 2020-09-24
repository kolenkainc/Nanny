namespace Nanny.Console.Printers
{
    public abstract class Printer : IPrinter
    {
        public abstract void Print(string line);
    }
}