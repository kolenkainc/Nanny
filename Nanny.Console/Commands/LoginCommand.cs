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
        private Scenario _jiraLogin;
        private Scenario _jiraDomain;
        private Scenario _githubToken;

        public LoginCommand(ApplicationContext db, ILogger<LoginCommand> logger, IPrinter printer, IScanner scanner) : base(printer)
        {
            _jiraDomain = new MissedKeyScenario(db, new Property{ Key = Constants.JiraDomain }, printer, scanner, logger);
            _jiraLogin = new MissedKeyScenario(db, new Property{ Key = Constants.JiraLogin }, printer, scanner, logger);
            _jiraToken = new MissedKeyScenario(db, new Property{ Key = Constants.JiraToken }, printer, scanner, logger);
            _githubToken = new MissedKeyScenario(db, new Property{ Key = Constants.GithubToken }, printer, scanner, logger);
        }

        public override void Execute()
        {
            _jiraDomain.Execute();
            _jiraLogin.Execute();
            _jiraToken.Execute();
            _githubToken.Execute();
        }

        public override Key Key()
        {
            return _key;
        }
        
        public override string HelpMessage()
        {
            return "pass tokens for Jira and Github";
        }
    }
}
