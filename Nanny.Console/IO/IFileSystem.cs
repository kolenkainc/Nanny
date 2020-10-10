using System.IO;
using Microsoft.Extensions.Logging;

namespace Nanny.Console.IO
{
    public interface IFileSystem
    {
        public DirectoryInfo InstallationDirectory();
        public DirectoryInfo CurrentDirectory();
        public bool IsGitRepository(DirectoryInfo info, ILogger _logger);
    }
}
