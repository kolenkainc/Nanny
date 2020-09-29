using System.Linq;
using Microsoft.Extensions.Logging;
using Nanny.Console.Database;
using Nanny.Console.IO;

namespace Nanny.Console.Commands
{
    public class WorklogCommand : Command
    {
        private Key _key = new Key("worklog", "w");
        private ILogger<WorklogCommand> _logger;
        private ApplicationContext _db;
        private string JIRA_TOKEN = "JiraToken";
        private IScanner _scanner;
        
        public WorklogCommand(IPrinter printer, IScanner scanner, ILogger<WorklogCommand> logger, ApplicationContext db) : base(printer)
        {
            _logger = logger;
            _db = db;
            _scanner = scanner;
        }

        public override void Execute()
        {
            _logger.LogInformation($"Is {JIRA_TOKEN} exists?");
            if (!_db.Properties.Any(p => p.Key == JIRA_TOKEN))
            {
                Printer.Print($"There is no stored {JIRA_TOKEN}. Please, login");
                _logger.LogWarning($"There is no stored {JIRA_TOKEN}");
                return;
            }
            _logger.LogInformation($"{JIRA_TOKEN} exist");

            _logger.LogInformation("Start to scan directory for git structure");
            
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
