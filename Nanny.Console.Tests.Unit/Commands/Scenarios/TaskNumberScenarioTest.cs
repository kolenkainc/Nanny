using System.Diagnostics;
using System.IO;
using Nanny.Console.IO;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands.Scenarios
{
    public class TaskNumberScenarioTest
    {
        [Fact]
        public void Some()
        {
            // AutoDetection();
        }

        public bool isGitFileSystem()
        {
            var fs = new FileSystem();
            var info = fs.CurrentDirectory();
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
