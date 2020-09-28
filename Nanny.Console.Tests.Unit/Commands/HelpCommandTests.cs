using System;
using Moq;
using Nanny.Console.Commands;
using Nanny.Console.IO;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class HelpCommandTests
    {
        private HelpCommand _command;
        private Mock<IPrinter> _printerMock;
        private Mock<CommandList> _commandListMock;
        private Mock<IServiceProvider> _serviceProvider;

        public HelpCommandTests()
        {
            _printerMock = new Mock<IPrinter>();
            _serviceProvider = new Mock<IServiceProvider>();
            _command = new HelpCommand(_serviceProvider.Object, _printerMock.Object);
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
