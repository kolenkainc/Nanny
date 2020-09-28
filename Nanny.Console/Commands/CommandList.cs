using System.Collections.Generic;

namespace Nanny.Console.Commands
{
    public class CommandList : List<Command>
    {
        private Command _default;
        
        public CommandList(VersionCommand version, HelpCommand help, LoginCommand login)
        {
            Add(version);
            Add(help);
            Add(login);
            _default = help;
        }

        public Command Find(string[] userInput)
        {
            if (userInput.Length == 0)
            {
                return _default;
            }

            Command candidate = Find(command => command.IsSuite(userInput[0]));

            return candidate == null ? _default : candidate;
        }
    }
}