using CovidService.Services.Github;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using CovidService.Controllers.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CovidServiceTest.Services
{
    [TestClass]
    public class GithubServiceTest
    {
        ILogger<GithubService> _logger = new Mock<ILogger<GithubService>>().Object;
        Mock<IConfiguration> _configurationMock;
        Mock<IDailyTimerService> _dailyTimerMock;

        public class HttpMessageHandlerMock : HttpMessageHandler
        {
            private Func<HttpResponseMessage> _action;

            public HttpMessageHandlerMock(Func<HttpResponseMessage> action)
            {
                _action = action;
            }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<HttpResponseMessage>(_action());
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x[It.Is<string>(s => s == "CovidCasesUrl")]).Returns("https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv");

            _dailyTimerMock = new Mock<IDailyTimerService>();
            _dailyTimerMock.SetupGet(x => x.IsNewDay).Returns(true);

            //_githubServiceMock.Setup(x => x.DownloadFile()).ReturnsAsync(new MemoryStream(Properties.Resources.Covid19ConfirmedUS));
        }

        [TestMethod]
        public async Task DownloadFile_Returns()
        {
            var handlerMock = new HttpMessageHandlerMock(() =>
            {
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
                };
            });
            var httpClient = new HttpClient(handlerMock);
            var service = new GithubService(_logger, _configurationMock.Object, httpClient, _dailyTimerMock.Object);

            var stream = await service.DownloadFile();

            Assert.AreEqual(1102184, stream.Length);
        }

        [TestMethod]
        public async Task DownloadFile_WrongURL_Throws()
        {
            var handlerMock = new HttpMessageHandlerMock(() => { throw (new Exception("Some http exception")); });
            var httpClient = new HttpClient(handlerMock);

            var service = new GithubService(_logger, _configurationMock.Object, httpClient, _dailyTimerMock.Object);
            await Assert.ThrowsExceptionAsync<GithubException>(
                () => service.DownloadFile()
            );
        }

        [TestMethod]
        public async Task DownloadFile_NoDownload_Returns()
        {
            int sendAsyncCounter = 0;
            var handlerMock = new HttpMessageHandlerMock(() =>
            {
                Interlocked.Increment(ref sendAsyncCounter);
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
                };
            });
            _dailyTimerMock.SetupGet(x => x.IsNewDay).Returns(false);

            var httpClient = new HttpClient(handlerMock);
            var service = new GithubService(_logger, _configurationMock.Object, httpClient, _dailyTimerMock.Object);

            var stream1 = await service.DownloadFile();
            var stream2 = await service.DownloadFile();

            Assert.AreEqual(1102184, stream1.Length);
            Assert.AreEqual(1102184, stream2.Length);
            Assert.AreEqual(1, sendAsyncCounter);
        }
    }
}
