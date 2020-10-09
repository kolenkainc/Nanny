using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using Nanny.Console.Commands.ExternalServices;
using Nanny.Console.Commands.Scenarios.TaskNumber;
using Nanny.Console.IO;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands.Scenarios
{
    public class TaskNumberScenarioTest
    {
        private Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private Mock<IPrinter> _printerMock = new Mock<IPrinter>();
        private Mock<IScanner> _scannerMock = new Mock<IScanner>();

        [Fact]
        public void CurrentDirectoryIsGitRepo_AutoDetection()
        {
            // Arrange
            var fsMock = new Mock<FileSystem>();
            var gitMock = new Mock<IGit>();
            fsMock
                .Setup(fs => fs.CurrentDirectory())
                .Returns(new DirectoryInfo("test"));
            fsMock
                .Setup(fs => fs.IsGitRepository(It.IsAny<DirectoryInfo>(), _loggerMock.Object))
                .Returns(true);
            var scenario = new TaskNumberScenario(fsMock.Object, _loggerMock.Object, _printerMock.Object, _scannerMock.Object, gitMock.Object);
            
            // Act
            var result = scenario.ExecuteScenario();
            
            // Assert

        }

        [Fact]
        public void CurrentDirectoryIsNotGitRepo_ManualDetection()
        {
            
        }
        
    }
}
