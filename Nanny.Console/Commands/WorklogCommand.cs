using Microsoft.Extensions.Logging;
using Nanny.Console.Commands.ExternalServices;
using Nanny.Console.Commands.Scenarios;
using Nanny.Console.Commands.Scenarios.TaskNumber;
using Nanny.Console.Database;
using Nanny.Console.IO;

namespace Nanny.Console.Commands
{
    public class WorklogCommand : Command
    {
        private Key _key = new Key("worklog", "w");
        private ILogger<WorklogCommand> _logger;
        private Scenario _jiraDomainScenario;
        private Scenario _jiraLoginScenario;
        private Scenario _jiraTokenScenario;
        private TaskNumberScenario _taskNumberScenario;
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
            _scanner = scanner;
            _jiraDomainScenario = new MissedKeyScenario(db, new Property{ Key = Constants.JiraDomain }, printer, scanner, logger);
            _jiraLoginScenario = new MissedKeyScenario(db, new Property{ Key = Constants.JiraLogin }, printer, scanner, logger);
            _jiraTokenScenario = new MissedKeyScenario(db, new Property{ Key = Constants.JiraToken }, printer, scanner, logger);
            var fs = new FileSystem();
            _taskNumberScenario = new TaskNumberScenario(fs, logger, printer, _scanner);
            _jira = jira;
        }

        public override void Execute()
        {
            _jiraDomainScenario.Execute();
            _jiraLoginScenario.Execute();
            _jiraTokenScenario.Execute();
            var task = _taskNumberScenario.ExecuteScenario();
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
                _logger.LogError("There is exception from Jira");
                _logger.LogError(e.Message);
                _logger.LogError("Worklog not added to Jira");
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
