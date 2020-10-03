using System.IO;
using System.Reflection;

namespace Nanny.Console.IO
{
    public class FileSystem
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

        public DirectoryInfo CurrentDirectory()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory());
        }
    }
}
