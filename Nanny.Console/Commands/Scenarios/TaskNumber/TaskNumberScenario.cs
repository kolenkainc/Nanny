using Microsoft.Extensions.Logging;
using Nanny.Console.Commands.ExternalServices;
using Nanny.Console.IO;

namespace Nanny.Console.Commands.Scenarios.TaskNumber
{
    public class TaskNumberScenario : Scenario
    {
        private IFileSystem _fileSystem;
        private ILogger _logger;
        private IPrinter _printer;
        private IScanner _scanner;
        private IGit _git;
        
        public TaskNumberScenario(IFileSystem fs, ILogger logger, IPrinter printer, IScanner scanner, IGit git)
        {
            _fileSystem = fs;
            _logger = logger;
            _printer = printer;
            _scanner = scanner;
            _git = git;
        }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public string ExecuteScenario()
        {
            if (_fileSystem.IsGitRepository(_fileSystem.CurrentDirectory(), _logger))
            {
                return AutoDetection();
            }
            else
            {
                return ManualDetection();
            }
        }

        public string AutoDetection()
        {
            return _git.TaskNumber();
        }

        public string ManualDetection()
        {
            _logger.LogInformation("Ask to task number");
            _printer.Print("Type task number");
            var task = _scanner.Scan();
            _logger.LogInformation($"Task number is: {task}");
            return task;
        }
    }
}
