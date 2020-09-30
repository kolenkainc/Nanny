using System.Linq;
using Microsoft.Extensions.Logging;
using Nanny.Console.Commands.Scenarios;
using Nanny.Console.Database;
using Nanny.Console.IO;

namespace Nanny.Console.Commands
{
    public class WorklogCommand : Command
    {
        private Key _key = new Key("worklog", "w");
        private ILogger<WorklogCommand> _logger;
        private ApplicationContext _db;
        private string JIRA_DOMAIN = "JiraDomain";
        private string JIRA_TOKEN = "JiraToken";
        private Scenario _jiraDomainScenario;
        private Scenario _jiraTokenScenario;
        private IScanner _scanner;
        
        public WorklogCommand(IPrinter printer, IScanner scanner, ILogger<WorklogCommand> logger, ApplicationContext db) : base(printer)
        {
            _logger = logger;
            _db = db;
            _scanner = scanner;
            _jiraDomainScenario = new MissedKeyScenario(db, new Property{ Key = JIRA_DOMAIN }, printer, scanner, logger);
            _jiraTokenScenario = new MissedKeyScenario(db, new Property{ Key = JIRA_TOKEN }, printer, scanner, logger);
        }

        public override void Execute()
        {
            _jiraDomainScenario.Execute();
            _jiraTokenScenario.Execute();
            _logger.LogInformation("Ask to task number");
            Printer.Print("Type task number");
            var task = _scanner.Scan();
            _logger.LogInformation($"Task number is: {task}");
        }

        public override Key Key()
        {
            return _key;
        }

        public override string HelpMessage()
        {
            return "create work log of task in Jira";
        }
    }
}
