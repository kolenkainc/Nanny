using Nanny.Console.Commands;
using NHamcrest.Core;
using Xunit;
using Assert = NHamcrest.XUnit.Assert;

namespace Nanny.Console.Tests.Business.Commands
{
    public class HelpCommandTests
    {
        private HelpCommand _command;

        public HelpCommandTests()
        {
            _command = new HelpCommand();
        }
        
        [Fact]
        public void ExecuteCommand_ReturnsHelpMessage()
        {
            Assert.That(_command.Execute(), new IsEqualMatcher<string>("Привет\nКак использовать Nanny мы еще не знаем"));
        }
    }
}
