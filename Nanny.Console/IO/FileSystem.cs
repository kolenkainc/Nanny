using System.IO;
using System.Reflection;

namespace Nanny.Console.IO
{
    public class FileSystem
    {
        public string WorkingDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
