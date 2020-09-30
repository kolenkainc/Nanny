using Microsoft.Extensions.Logging;
using Moq;
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
        private Mock<ApplicationContext> _dbMock;
        private Mock<ILogger<WorklogCommand>> _loggerMock;
        private Mock<IJira> _jiraMock;

        public WorklogCommandTests()
        {
            _printerMock = new Mock<IPrinter>();
            _scannerMock = new Mock<IScanner>();
            _dbMock = new Mock<ApplicationContext>();
            _loggerMock = new Mock<ILogger<WorklogCommand>>();
            _jiraMock = new Mock<IJira>();
            _command = new WorklogCommand(
                _printerMock.Object,
                _scannerMock.Object,
                _loggerMock.Object,
                _dbMock.Object,
                _jiraMock.Object
            );
        }
        
        [Fact]
        public void ExecuteCommand_WithoutTokens_AskTokens()
        {
            
        }
    }
}
