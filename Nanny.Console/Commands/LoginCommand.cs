using Microsoft.Extensions.Logging;
using Nanny.Console.Commands.Scenarios;
using Nanny.Console.Database;
using Nanny.Console.IO;

namespace Nanny.Console.Commands
{
    public class LoginCommand : Command
    {
        private Key _key = new Key("login", "l");
        private Scenario _jiraToken;
        private Scenario _jiraDomain;
        private Scenario _githubToken;

        public LoginCommand(ApplicationContext db, ILogger<LoginCommand> logger, IPrinter printer, IScanner scanner) : base(printer)
        {
            _jiraDomain = new MissedKeyScenario(db, new Property{ Key = "JiraDomain" }, printer, scanner, logger);
            _jiraToken = new MissedKeyScenario(db, new Property{ Key = "JiraToken" }, printer, scanner, logger);
            _githubToken = new MissedKeyScenario(db, new Property{ Key = "GithubToken" }, printer, scanner, logger);
        }

        public override void Execute()
        {
            _jiraDomain.Execute();
            _jiraToken.Execute();
            _githubToken.Execute();
        }

        public override Key Key()
        {
            return _key;
        }
    }
}
