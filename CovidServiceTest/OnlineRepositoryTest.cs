using CovidService.Models;
using CovidService.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace CovidServiceTest
{
    [TestClass]
    public class OnlineRepositoryTest
    {
        ILogger<OnlineRepository> _logger = new Mock<ILogger<OnlineRepository>>().Object;

        [TestMethod]
        public void GetStates_ReturnsList()
        {
            var file = new OnlineRepository(_logger);
            var counties = file.GetStates();

            Assert.AreEqual(53, counties.Count());
            Assert.AreEqual("Alabama", counties[0].Name);
        }

        [TestMethod]
        public void GetCounties_ReturnsList()
        {
            var file = new OnlineRepository(_logger);
            var counties = file.GetCounties();

            Assert.AreEqual(3258, counties.Count());
            Assert.AreEqual("Barbour, Alabama, US", counties[2].CombinedKey);
        }
    }
}
