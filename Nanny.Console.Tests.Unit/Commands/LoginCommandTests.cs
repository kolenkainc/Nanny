using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Nanny.Console.Commands;
using Nanny.Console.Database;
using NHamcrest;
using Xunit;
using Assert = NHamcrest.XUnit.Assert;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class LoginCommandTests
    {
        [Fact]
        public void ExecuteCommand_WithEmptyDb_ReturnsErrorMessage()
        {
            // Arrange
            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(m => m.Properties).ReturnsDbSet(new List<Property>());
            Mock<ILogger<LoginCommand>> loggerMock = new Mock<ILogger<LoginCommand>>();
            LoginCommand command = new LoginCommand(mockContext.Object, loggerMock.Object);
            
            // Act and Assert
            Assert.That(command.Execute(), Is.EqualTo("Please, pass JiraToken"));
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
            LoginCommand command = new LoginCommand(mockContext.Object, loggerMock.Object);
            
            // Act and Assert
            Assert.That(command.Execute(), Is.EqualTo("You already have JiraToken"));
        }
    }
}
