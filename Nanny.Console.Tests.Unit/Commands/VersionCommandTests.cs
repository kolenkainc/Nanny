using Moq;
using Nanny.Console.Commands;
using Nanny.Console.IO;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class VersionCommandTests
    {
        private VersionCommand _versionCommand;
        private Mock<IPrinter> _printerMock;

        public VersionCommandTests()
        {
            _printerMock = new Mock<IPrinter>();
            _versionCommand = new VersionCommand(_printerMock.Object);
        }
        
        [Fact]
        public void ExecuteCommand_ReturnsVersionMessage()
        {
            // Act
            _versionCommand.Execute();
            
            // Assert
            _printerMock.Verify(
                p => p.Print("Nanny version: 1.0.1.2"),
                Times.Once
            );
        }
    }
}
