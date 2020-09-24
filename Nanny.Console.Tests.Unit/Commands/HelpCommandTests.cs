using Moq;
using Nanny.Console.Commands;
using Nanny.Console.Printers;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class HelpCommandTests
    {
        private HelpCommand _command;
        private Mock<IPrinter> _printerMock;

        public HelpCommandTests()
        {
            _printerMock = new Mock<IPrinter>();
            _command = new HelpCommand(_printerMock.Object);
        }
        
        [Fact]
        public void ExecuteCommand_ReturnsHelpMessage()
        {
            // Act
            _command.Execute();
            
            // Assert
            _printerMock.Verify(
                m => m.Print("Привет\nКак использовать Nanny мы еще не знаем"),
                Times.Once
            );
        }
    }
}
