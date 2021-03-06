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

        [Fact]
        public void CurrentDirectoryIsGitRepo_AutoDetection()
        {
            // Arrange
            var scannerMock = new Mock<IScanner>();
            var fsMock = new Mock<FileSystem>();
            var gitMock = new Mock<IGit>();
            fsMock
                .Setup(fs => fs.CurrentDirectory())
                .Returns(new DirectoryInfo("test"));
            fsMock
                .Setup(fs => fs.IsGitRepository(It.IsAny<DirectoryInfo>(), _loggerMock.Object))
                .Returns(true);
            gitMock
                .Setup(git => git.TaskNumber())
                .Returns("Test-Task");
            var scenario = new TaskNumberScenario(fsMock.Object, _loggerMock.Object, _printerMock.Object, scannerMock.Object, gitMock.Object);

            // Act and Assert
            Assert.Equal("Test-Task", scenario.ExecuteScenario());
        }

        [Fact]
        public void CurrentDirectoryIsNotGitRepo_ManualDetection()
        {
            // Arrange
            var scannerMock = new Mock<IScanner>();
            scannerMock
                .Setup(s => s.Scan())
                .Returns("Test-Task");
            var fsMock = new Mock<FileSystem>();
            var gitMock = new Mock<IGit>();
            fsMock
                .Setup(fs => fs.CurrentDirectory())
                .Returns(new DirectoryInfo("test"));
            fsMock
                .Setup(fs => fs.IsGitRepository(It.IsAny<DirectoryInfo>(), _loggerMock.Object))
                .Returns(false);
            var scenario = new TaskNumberScenario(fsMock.Object, _loggerMock.Object, _printerMock.Object, scannerMock.Object, gitMock.Object);

            // Act and Assert
            Assert.Equal("Test-Task", scenario.ExecuteScenario());
        }
        
    }
}
