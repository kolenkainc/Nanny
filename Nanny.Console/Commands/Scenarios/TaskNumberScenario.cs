using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
using Nanny.Console.IO;

namespace Nanny.Console.Commands.Scenarios
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
            if (isGitFileSystem())
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
            // start.FileName = "git rev-parse --abbrev-ref HEAD";
            start.FileName = "git";
            start.Arguments = "rev-parse --abbrev-ref HEAD";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            string result;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                    System.Console.WriteLine(result);
                }
            }
            return result;
        }

        public string ManualDetection()
        {
            _logger.LogInformation("Ask to task number");
            _printer.Print("Type task number");
            var task = _scanner.Scan();
            _logger.LogInformation($"Task number is: {task}");
            return task;
        }

        private bool isGitFileSystem()
        {
            var info = _fileSystem.CurrentDirectory();
            while (info != null && info.Parent != null && info.Parent.Exists)
            {
                foreach (var directoryInfo in info.GetDirectories())
                {
                    if (directoryInfo.Extension == ".git")
                    {
                        return true;
                    }
                }

                info = info.Parent;
            }

            return false;
        }
    }
}
