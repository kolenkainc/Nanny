using System;
using Microsoft.Extensions.Logging;
using Moq;
using Nanny.Console.Commands;
using Nanny.Console.Commands.ExternalServices;
using Nanny.Console.Database;
using Nanny.Console.IO;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class CommandListTests
    {
        private CommandList _commandList;

        public CommandListTests()
        {
            var printerMock = new Mock<IPrinter>();
            var scannerMock = new Mock<IScanner>();
            var dbMock = new Mock<ApplicationContext>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var jiraMock = new Mock<IJira>();
            _commandList = new CommandList(
                new VersionCommand(printerMock.Object),
                new HelpCommand(serviceProviderMock.Object, printerMock.Object, new Mock<ILogger<HelpCommand>>().Object),
                new LoginCommand(dbMock.Object, new Mock<ILogger<LoginCommand>>().Object, printerMock.Object, scannerMock.Object),
                new WorklogCommand(printerMock.Object, scannerMock.Object, new Mock<ILogger<WorklogCommand>>().Object, dbMock.Object, jiraMock.Object, new Mock<IGit>().Object) 
            );
        }
        
        [Fact]
        public void FindCommand_ViaShortKey_ShouldFindSuccessfully()
        {
            // Arrange
            var userInput = new[] {"--v"};
            
            // Act
            var result = _commandList.Find(userInput);

            // Assert
            Assert.IsType<VersionCommand>(result);
        }
        
        [Fact]
        public void FindCommand_ViaLongKey_ShouldFindSuccessfully()
        {
            // Arrange
            var userInput = new[] {"--version"};
            
            // Act
            var result = _commandList.Find(userInput);

            // Assert
            Assert.IsType<VersionCommand>(result);
        }
        
        [Fact]
        public void FindCommand_ViaNotExistedKey_ShouldFindHelpCommand()
        {
            // Arrange
            var userInput = new[] {"--abracadabra"};
            
            // Act
            var result = _commandList.Find(userInput);

            // Assert
            Assert.IsType<HelpCommand>(result);
        }
        
        [Fact]
        public void FindCommand_ViaEmptyInput_ShouldFindHelpCommand()
        {
            // Arrange
            var userInput = new string[0];
            
            // Act
            var result = _commandList.Find(userInput);

            // Assert
            Assert.IsType<HelpCommand>(result);
        }
    }
}
