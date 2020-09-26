using System.Collections.Generic;

namespace Nanny.Console.Commands
{
    public class CommandList : List<Command>
    {
        public CommandList(VersionCommand version, HelpCommand help, LoginCommand login)
        {
            Add(version);
            Add(help);
            Add(login);
        }

        public Command Find(string[] userInput, Command defaultValue)
        {
            if (userInput.Length == 0)
            {
                return defaultValue;
            }

            Command candidate = Find(command => command.IsSuite(userInput[0]));

            return candidate == null ? defaultValue : candidate;
        }
    }
}