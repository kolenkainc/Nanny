using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Nanny.Console.Commands.ExternalServices
{
    public class Git : IGit
    {
        private ILogger<IGit> _logger;

        public Git(ILogger<IGit> logger)
        {
            _logger = logger;
        }
        
        public string TaskNumber()
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
    }
}
