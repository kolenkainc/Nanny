using System.Linq;
using Microsoft.Extensions.Logging;
using Nanny.Console.Database;
using Nanny.Console.Printers;

namespace Nanny.Console.Commands
{
    public class LoginCommand : Command
    {
        private Key _key = new Key("login", "l");
        private ApplicationContext _db;
        private ILogger<LoginCommand> _logger;

        public LoginCommand(ApplicationContext db, ILogger<LoginCommand> logger, IPrinter printer) : base(printer)
        {
            _db = db;
            _logger = logger;
        }

        public override void Execute()
        {
            _logger.LogInformation("Check JiraToken");
            Property property = _db.Properties.FirstOrDefault(p => p.Key == "JiraToken");
            if (property == null)
            {
                _logger.LogInformation("There is no JiraToken in db");
                Printer.Print("Please, pass JiraToken");
            }
            else
            {
                Printer.Print("You already have JiraToken");
            }
        }

        public override Key Key()
        {
            return _key;
        }
    }
}
