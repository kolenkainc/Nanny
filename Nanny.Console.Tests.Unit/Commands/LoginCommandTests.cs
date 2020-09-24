using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Nanny.Console.Commands;
using Nanny.Console.Database;
using Nanny.Console.Printers;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class LoginCommandTests
    {
        private Mock<IPrinter> _printerMock;

        public LoginCommandTests()
        {
            _printerMock = new Mock<IPrinter>();
        }

            [Fact]
        public void ExecuteCommand_WithEmptyDb_ReturnsErrorMessage()
        {
            // Arrange
            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(m => m.Properties).ReturnsDbSet(new List<Property>());
            Mock<ILogger<LoginCommand>> loggerMock = new Mock<ILogger<LoginCommand>>();
            LoginCommand command = new LoginCommand(mockContext.Object, loggerMock.Object, _printerMock.Object);
            
            // Act
            command.Execute();
            
            // Assert
            _printerMock.Verify(m => m.Print("Please, pass JiraToken"), Times.Once);
        }

        [Fact]
        public void ExecuteCommand_WithJiraToken_ReturnsOkMessage()
        {
            // Arrange
            var mockContext = new Mock<ApplicationContext>();
            var props = new List<Property>
            {
                new Property
                {
                    Key = "JiraToken",
                    Value = "123"
                }
            };
            mockContext.Setup(m => m.Properties).ReturnsDbSet(props);
            Mock<ILogger<LoginCommand>> loggerMock = new Mock<ILogger<LoginCommand>>();
            LoginCommand command = new LoginCommand(mockContext.Object, loggerMock.Object, _printerMock.Object);
            
            // Act
            command.Execute();
            
            // Assert
            _printerMock.Verify(m => m.Print("You already have JiraToken"), Times.Once);
        }
    }
}
