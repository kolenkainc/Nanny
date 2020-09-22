using System.Linq;
using Microsoft.Extensions.Logging;
using Nanny.Console.Database;

namespace Nanny.Console.Commands
{
    public class LoginCommand : Command
    {
        private Key _key = new Key("login", "l");
        private ApplicationContext _db;
        private Logger<LoginCommand> _logger;

        public LoginCommand(ApplicationContext db, Logger<LoginCommand> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override string Output()
        {
            _logger.LogInformation("Check JiraToken");
            Property property = _db.Properties.FirstOrDefault(p => p.Key == "JiraToken");
            if (property == null)
            {
                _logger.LogInformation("There is no JiraToken in db");
                return "Please, pass JiraToken";
            }

            return "You already have JiraToken";
        }

        public override Key Key()
        {
            return _key;
        }
    }
}
