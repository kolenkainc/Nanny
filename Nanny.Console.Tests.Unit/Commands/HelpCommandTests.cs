using Nanny.Console.Commands;
using NHamcrest;
using Xunit;
using Assert = NHamcrest.XUnit.Assert;

namespace Nanny.Console.Tests.Unit.Commands
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
            Assert.That(_command.Execute(), Is.EqualTo("Привет\nКак использовать Nanny мы еще не знаем"));
        }
    }
}
