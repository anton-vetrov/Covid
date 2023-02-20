using CovidService.Models;
using CovidService.Repositories;
using CovidService.Services.Github;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace CovidServiceTest.Repositories
{
    [TestClass]
    public class OnlineRepositoryTest
    {
        ILogger<OnlineRepository> _logger = new Mock<ILogger<OnlineRepository>>().Object;
        Mock<ILogger<GithubService>> _githubServiceLogger;
        Mock<IConfiguration> _configurationMock;
        Mock<IGithubService> _githubServiceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _githubServiceLogger = new Mock<ILogger<GithubService>>();
            _configurationMock = new Mock<IConfiguration>();
            _githubServiceMock = new Mock<IGithubService>();
            _githubServiceMock.Setup(x => x.DownloadFile()).ReturnsAsync(new MemoryStream(Properties.Resources.Covid19ConfirmedUS));
        }

        [TestMethod]
        public async Task GetStatesAsync_ReturnsList()
        {
            
            var file = new OnlineRepository(_logger, _githubServiceMock.Object);
            var state = await file.GetStateAsync("Alabama");

            Assert.AreEqual(69, state.Counties.Count());
            Assert.AreEqual("Alabama", state.Name);
        }

        [TestMethod]
        public async Task GetStatesAsync_RealHttpRequest_ReturnsList()
        {
            var httpClient = new HttpClient();

            _configurationMock.Setup(x => x[It.Is<string>(s => s == "CovidCasesUrl")]).Returns("https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv");

            var githubService = new GithubService(_githubServiceLogger.Object, _configurationMock.Object, httpClient);
            var file = new OnlineRepository(_logger, githubService);
            var state = await file.GetStateAsync("Alabama");

            Assert.AreEqual(69, state.Counties.Count());
            Assert.AreEqual("Alabama", state.Name);
            var anotherState = await file.GetStateAsync("California");
            Assert.AreEqual(60, anotherState.Counties.Count());
            Assert.AreEqual("California", anotherState.Name);

            _githubServiceLogger.Verify(
                // TODO Verify log string
                x => x.Log<GithubService>(
                    It.Is<LogLevel>(x => x == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.IsAny<GithubService>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<GithubService, Exception, string>>()
                ),
                Times.AtLeastOnce
            );
        }

        [TestMethod]
        public async Task GetStateAsync_NotFound_ReturnsNull()
        {
            var file = new OnlineRepository(_logger, _githubServiceMock.Object);
            var state = await file.GetStateAsync("AAA");

            Assert.AreEqual(null, state);
        }

        [TestMethod]
        public async Task GetCountiesAsync_ReturnsList()
        {
            var file = new OnlineRepository(_logger, _githubServiceMock.Object);
            var counties = await file.GetCountiesAsync();

            Assert.AreEqual(3258, counties.Count());
            Assert.AreEqual("Barbour, Alabama, US", counties[2].CombinedKey);
        }
    }
}
