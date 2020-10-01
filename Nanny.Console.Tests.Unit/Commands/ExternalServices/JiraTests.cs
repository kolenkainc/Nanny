using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Moq.Protected;
using Nanny.Console.Commands.ExternalServices;
using Nanny.Console.Database;
using Xunit;

namespace Nanny.Console.Tests.Unit.Commands.ExternalServices
{
    public class JiraTests
    {
        private Mock<ILogger<IJira>> _loggerMock;

        public JiraTests()
        {
            _loggerMock = new Mock<ILogger<IJira>>();
        }
        
        [Fact]
        public void SendWorklog_WithRightParameters_InvokeHttpClientWithParameters()
        {
            // Arrange
            var mockDb = new Mock<ApplicationContext>();
            var props = new List<Property>
            {
                new Property {Key = "JiraDomain", Value = "http://test.test"},
                new Property {Key = "JiraLogin", Value = "test"},
                new Property {Key = "JiraToken", Value = "test"}
            };
            mockDb.Setup(m => m.Properties).ReturnsDbSet(props);

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'message':'OK'}]"),
                })
                .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.test/"),
            };
            var expectedUri = new Uri("http://test.test/rest/api/3/issue/task/worklog");

            var jira = new Jira(httpClient, mockDb.Object, _loggerMock.Object);

            // Act
            jira.Worklog("task", "1d");

            // Assert
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post  // we expected a POST request
                        && req.RequestUri == expectedUri // to this uri
                        && Equals(req.Headers.Authorization, new AuthenticationHeaderValue(
                            "Basic", Convert.ToBase64String(
                                Encoding.ASCII.GetBytes(
                                    "test:test"))))
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public void SendWorklog_WithWrongParameters_HttpClientShouldReturnError()
        {
            // Arrange
            var mockDb = new Mock<ApplicationContext>();
            var props = new List<Property>
            {
                new Property {Key = "JiraDomain", Value = "http://test.test"},
                new Property {Key = "JiraLogin", Value = "test"},
                new Property {Key = "JiraToken", Value = "test"}
            };
            mockDb.Setup(m => m.Properties).ReturnsDbSet(props);

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("[{'message':'Error'}]"),
                })
                .Verifiable();
            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.test/"),
            };

            var jira = new Jira(httpClient, mockDb.Object, _loggerMock.Object);

            // Act and Assert
            Assert.Throws<JiraException>(() => jira.Worklog("task", "1d"));
        }
    }
}
