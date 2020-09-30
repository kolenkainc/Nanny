using Nanny.Console.Commands.ExternalServices;
using NHamcrest;
using Xunit;
using Assert = NHamcrest.XUnit.Assert;

namespace Nanny.Console.Tests.Unit.Commands.ExternalServices
{
    public class WorklogRequestTests
    {
        [Fact]
        public void Some()
        {
            // Arrange
            var worklogRequest = new WorklogRequest("1d");
            
            // Act and Assert
            Assert.That(worklogRequest.ParseHuman().TotalSeconds, Is.EqualTo(24.0 * 60 * 60));
        }
    }
}
