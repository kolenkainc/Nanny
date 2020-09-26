using System.Linq;
using Microsoft.Extensions.Logging;
using Nanny.Console.Database;
using Nanny.Console.IO;

namespace Nanny.Console.Commands.Scenarios
{
    public class MissedKeyScenario : Scenario
    {
        private ApplicationContext _db;
        private Property _property;
        private IPrinter _printer;
        private IScanner _scanner;
        private ILogger _logger;
        
        public MissedKeyScenario(ApplicationContext db, Property property, IPrinter printer, IScanner scanner, ILogger logger)
        {
            _db = db;
            _property = property;
            _printer = printer;
            _scanner = scanner;
            _logger = logger;
        }

        public override void Execute()
        {
            _logger.LogInformation($"Check {_property.Key}");
            Property propertyInDb = _db.Properties.FirstOrDefault(p => p.Key == _property.Key);
            if (propertyInDb == null)
            {
                _logger.LogInformation($"There is no {_property.Key} in db");
                _printer.Print($"Please, pass {_property.Key}");
                _db.Properties.Add(new Property
                {
                    Key = _property.Key,
                    Value = validateJiraToken(_scanner.Scan())
                });
                _db.SaveChanges();
            }
            else
            {
                _printer.Print($"You already have {_property.Key}");
            }
        }

        private string validateJiraToken(string candidate)
        {
            return candidate;
        }
    }
}
