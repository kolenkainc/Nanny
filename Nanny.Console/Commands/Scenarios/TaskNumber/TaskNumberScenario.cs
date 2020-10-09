using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
using Nanny.Console.IO;

namespace Nanny.Console.Commands.Scenarios.TaskNumber
{
    public class TaskNumberScenario : Scenario
    {
        private FileSystem _fileSystem;
        private ILogger _logger;
        private IPrinter _printer;
        private IScanner _scanner;
        
        public TaskNumberScenario(FileSystem fs, ILogger logger, IPrinter printer, IScanner scanner)
        {
            _fileSystem = fs;
            _logger = logger;
            _printer = printer;
            _scanner = scanner;
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
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "git";
            start.Arguments = "rev-parse --abbrev-ref HEAD";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    var result = reader.ReadToEnd();
                    _logger.LogInformation($"Task number \"{result}\" was detected automatically");
                    return result;
                }
            }
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
