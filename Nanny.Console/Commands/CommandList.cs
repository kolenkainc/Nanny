using System.Collections.Generic;

namespace Nanny.Console.Commands
{
    public class CommandList : List<Command>
    {
        public Command Find(string[] userInput, Command defaultValue)
        {
            if (userInput.Length == 0)
            {
                return defaultValue;
            }

            Command candidate = Find(command => command.IsSuite(userInput[0]));

            if (candidate == null)
            {
                return defaultValue;
            }
            else
            {
                return candidate;
            }
        }
    }
}