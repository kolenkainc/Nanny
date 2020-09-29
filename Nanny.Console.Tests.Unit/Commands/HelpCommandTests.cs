using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Nanny.Console.Commands;
using Nanny.Console.Database;
using Nanny.Console.IO;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands
{
    public class HelpCommandTests
    {
        private HelpCommand _command;
        private Mock<IPrinter> _printerMock;
        private Mock<IServiceProvider> _serviceProvider;

        public HelpCommandTests()
        {
            _printerMock = new Mock<IPrinter>();
            var fakeVersionCommand = new VersionCommand(_printerMock.Object);
            var fakeHelpCommand = new HelpCommand(null, _printerMock.Object);
            var fakeLoginCommand = new LoginCommand(
                new Mock<ApplicationContext>().Object,
                new Mock<ILogger<LoginCommand>>().Object,
                _printerMock.Object,
                new Mock<IScanner>().Object
            );
            var fakeWorklogCommand = new WorklogCommand(
                _printerMock.Object,
                new Mock<ILogger<WorklogCommand>>().Object,
                new Mock<ApplicationContext>().Object
            );

            _serviceProvider = new Mock<IServiceProvider>();
            _serviceProvider
                .Setup(x => x.GetService(typeof(CommandList)))
                .Returns(new CommandList(fakeVersionCommand, fakeHelpCommand, fakeLoginCommand, fakeWorklogCommand));

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(_serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            _serviceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            _command = new HelpCommand(_serviceProvider.Object, _printerMock.Object);
        }
        
        [Fact]
        public void ExecuteCommand_ReturnsHelpMessage()
        {
            // Act
            _command.Execute();
            
            // Assert
            _printerMock.Verify(
                    m => m.Print(
                        $"Commands:{Environment.NewLine}" +
                        $"  nanny --version or --v  # display version of nanny{Environment.NewLine}" +
                        $"  nanny --help or --h     # describe available commands{Environment.NewLine}" +
                        $"  nanny --login or --l    # pass tokens for Jira and Github{Environment.NewLine}" +
                        $"  nanny --worklog or --w  # create work log of task in Jira{Environment.NewLine}"
                    ),
                Times.Once
            );
        }
    }
}
