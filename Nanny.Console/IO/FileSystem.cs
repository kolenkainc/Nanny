using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nanny.Console.IO
{
    public class FileSystem : IFileSystem
    {
        public DirectoryInfo InstallationDirectory()
        {
            var candidate = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (candidate == null)
            {
                throw new IOException("Cannot get installation directory");
            }
            return new DirectoryInfo(candidate);
        }

        public virtual DirectoryInfo CurrentDirectory()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory());
        }

        public virtual bool IsGitRepository(DirectoryInfo info, ILogger _logger)
        {
            while (info != null && info.Parent != null && info.Parent.Exists)
            {
                foreach (var directoryInfo in info.GetDirectories())
                {
                    _logger.LogDebug($"Scan {directoryInfo.Name}");
                    if (directoryInfo.Extension == ".git")
                    {
                        _logger.LogInformation("Git directory structure was detected");
                        return true;
                    }
                }

                info = info.Parent;
            }

            _logger.LogInformation("Current directory is not git repository");
            return false;
        }
    }
}
