using System;

namespace Nanny.Console.Commands.ExternalServices
{
    public class JiraException : Exception
    {
        public JiraException(string message) : base(message)
        {
        }
    }
}
