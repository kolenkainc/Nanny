namespace Nanny.Console.Commands.ExternalServices
{
    public interface IJira
    {
        void Worklog(string task, string worklog);
    }
}
