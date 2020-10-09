using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Nanny.Console.Commands;
using Nanny.Console.Commands.ExternalServices;
using Nanny.Console.Database;
using Nanny.Console.IO;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class WorklogCommandTests
    {
        private WorklogCommand _command;
        private Mock<IPrinter> _printerMock;
        private Mock<IScanner> _scannerMock;
        private Mock<ILogger<WorklogCommand>> _loggerMock;
        private Mock<IJira> _jiraMock;
        private Mock<IGit> _gitMock = new Mock<IGit>();

        public WorklogCommandTests()
        {
            _printerMock = new Mock<IPrinter>();
            _scannerMock = new Mock<IScanner>();
            _loggerMock = new Mock<ILogger<WorklogCommand>>();
            _jiraMock = new Mock<IJira>();
        }

        [Fact]
        public void ExecuteCommand_WithoutTokens_AskTokens()
        {
            // Arrange
            var mockDb = new Mock<ApplicationContext>();
            mockDb.Setup(m => m.Properties).ReturnsDbSet(new List<Property>());
            _scannerMock.SetupSequence(scanner => scanner.Scan())
                .Returns("test_jira_domain")
                .Returns("test_jira_login")
                .Returns("test_jira_token")
                .Returns("task_number")
                .Returns("worklog_line");
            var command = new WorklogCommand(
                _printerMock.Object,
                _scannerMock.Object,
                _loggerMock.Object,
                mockDb.Object,
                _jiraMock.Object,
                _gitMock.Object
            );
            
            // Act
            command.Execute();
            
            // Assert
            _printerMock.Verify(m => m.Print("Please, pass JiraDomain"), Times.Once);
            _printerMock.Verify(m => m.Print("Please, pass JiraLogin"), Times.Once);
            _printerMock.Verify(m => m.Print("Please, pass JiraToken"), Times.Once);
            _printerMock.Verify(m => m.Print("Type task number"), Times.Once);
            _printerMock.Verify(m => m.Print("Type worklog for this task"), Times.Once);
            _scannerMock.Verify(m => m.Scan(), Times.Exactly(5));
        }

        [Fact]
        public void ExecuteCommand_WithTokens_AskTaskAndWorklogLine()
        {
            // Arrange
            var mockDb = new Mock<ApplicationContext>();
            var props = new List<Property>
            {
                new Property {Key = "JiraDomain", Value = "123"},
                new Property {Key = "JiraLogin", Value = "123"},
                new Property {Key = "JiraToken", Value = "123"}
            };
            mockDb.Setup(m => m.Properties).ReturnsDbSet(props);
            Mock<ILogger<LoginCommand>> loggerMock = new Mock<ILogger<LoginCommand>>();
            _scannerMock.SetupSequence(scanner => scanner.Scan())
                .Returns("task_number")
                .Returns("worklog_line");
            var command = new WorklogCommand(
                _printerMock.Object,
                _scannerMock.Object,
                _loggerMock.Object,
                mockDb.Object,
                _jiraMock.Object,
                _gitMock.Object
            );
            
            // Act
            command.Execute();
            
            // Assert
            _printerMock.Verify(m => m.Print("Type task number"), Times.Once);
            _printerMock.Verify(m => m.Print("Type worklog for this task"), Times.Once);
            _scannerMock.Verify(m => m.Scan(), Times.Exactly(2));
        }
    }
}
