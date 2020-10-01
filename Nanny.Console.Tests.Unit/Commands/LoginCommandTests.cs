using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Nanny.Console.Commands;
using Nanny.Console.Database;
using Nanny.Console.IO;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class LoginCommandTests
    {
        private Mock<IPrinter> _printerMock;
        private Mock<IScanner> _scannerMock;

        public LoginCommandTests()
        {
            _printerMock = new Mock<IPrinter>();
            _scannerMock = new Mock<IScanner>();
        }

        [Fact]
        public void ExecuteCommand_WithEmptyDb_ReturnsErrorMessage()
        {
            // Arrange
            var mockDb = new Mock<ApplicationContext>();
            mockDb.Setup(m => m.Properties).ReturnsDbSet(new List<Property>());
            Mock<ILogger<LoginCommand>> loggerMock = new Mock<ILogger<LoginCommand>>();
            _scannerMock.SetupSequence(scanner => scanner.Scan())
                .Returns("test_jira_domain")
                .Returns("test_jira_login")
                .Returns("test_jira_token")
                .Returns("test_github_token");
            LoginCommand command = new LoginCommand(mockDb.Object, loggerMock.Object, _printerMock.Object, _scannerMock.Object);
            
            // Act
            command.Execute();
            
            // Assert
            _printerMock.Verify(m => m.Print("Please, pass JiraDomain"), Times.Once);
            _printerMock.Verify(m => m.Print("Please, pass JiraLogin"), Times.Once);
            _printerMock.Verify(m => m.Print("Please, pass JiraToken"), Times.Once);
            _printerMock.Verify(m => m.Print("Please, pass GithubToken"), Times.Once);
            _scannerMock.Verify(m => m.Scan(), Times.Exactly(4));
        }

        [Fact]
        public void ExecuteCommand_WithTokens_ReturnsOkMessage()
        {
            // Arrange
            var mockDb = new Mock<ApplicationContext>();
            var props = new List<Property>
            {
                new Property {Key = "JiraDomain", Value = "123"},
                new Property {Key = "JiraToken", Value = "123"},
                new Property {Key = "GithubToken", Value = "123"}
            };
            mockDb.Setup(m => m.Properties).ReturnsDbSet(props);
            Mock<ILogger<LoginCommand>> loggerMock = new Mock<ILogger<LoginCommand>>();
            LoginCommand command = new LoginCommand(mockDb.Object, loggerMock.Object, _printerMock.Object, _scannerMock.Object);
            
            // Act
            command.Execute();
            
            // Assert
            _printerMock.Verify(m => m.Print("You already have JiraDomain"), Times.Once);
            _printerMock.Verify(m => m.Print("You already have JiraToken"), Times.Once);
            _printerMock.Verify(m => m.Print("You already have GithubToken"), Times.Once);
        }
    }
}
