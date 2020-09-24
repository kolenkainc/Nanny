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
        private LoginCommand _command;

        public LoginCommandTests()
        {
            var mockContext = new Mock<ApplicationContext>();
            mockContext.Setup(m => m.Properties).ReturnsDbSet(new List<Property>());
            Mock<ILogger<LoginCommand>> loggerMock = new Mock<ILogger<LoginCommand>>();
            _command = new LoginCommand(mockContext.Object, loggerMock.Object);
        }

        [Fact]
        public void ExecuteCommand_WithEmptyDb_ReturnsErrorMessage()
        {
            Assert.That(_command.Execute(), Is.EqualTo("Please, pass JiraToken"));
        }
    }
}
