using Microsoft.Extensions.Logging;
using Nanny.Console.Commands.ExternalServices;
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
        private Scenario _jiraDomainScenario;
        private Scenario _jiraLoginScenario;
        private Scenario _jiraTokenScenario;
        private IScanner _scanner;
        private IJira _jira;
        
        public WorklogCommand(
            IPrinter printer,
            IScanner scanner,
            ILogger<WorklogCommand> logger,
            ApplicationContext db,
            IJira jira
        ) : base(printer)
        {
            _logger = logger;
            _db = db;
            _scanner = scanner;
            _jiraDomainScenario = new MissedKeyScenario(db, new Property{ Key = Constants.JiraDomain }, printer, scanner, logger);
            _jiraLoginScenario = new MissedKeyScenario(db, new Property{ Key = Constants.JiraLogin }, printer, scanner, logger);
            _jiraTokenScenario = new MissedKeyScenario(db, new Property{ Key = Constants.JiraToken }, printer, scanner, logger);
            _jira = jira;
        }

        public override void Execute()
        {
            _jiraDomainScenario.Execute();
            _jiraTokenScenario.Execute();
            _logger.LogInformation("Ask to task number");
            Printer.Print("Type task number");
            var task = _scanner.Scan();
            _logger.LogInformation($"Task number is: {task}");
            _logger.LogInformation("Ask to worklog");
            Printer.Print("Type worklog for this task");
            var worklog = _scanner.Scan();
            _logger.LogInformation($"Worklog line is: {worklog}");
            try
            {
                _jira.Worklog(task, worklog);
                _logger.LogInformation($"Worklog {worklog} was successfully added for task {task}");
            }
            catch (JiraException e)
            {
                _logger.LogError("There is exception from Jira: {0e}. Worklog not added to Jira", e);
            }
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
