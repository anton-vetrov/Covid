using CovidService.Models;
using CovidService.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace CovidServiceTest.Repositories
{
    [TestClass]
    public class OnlineRepositoryTest
    {
        ILogger<OnlineRepository> _logger = new Mock<ILogger<OnlineRepository>>().Object;

        [TestMethod]
        public void GetStates_ReturnsList()
        {
            var file = new OnlineRepository(_logger);
            var state = file.GetState("Alabama");

            Assert.AreEqual(69, state.Counties.Count());
            Assert.AreEqual("Alabama", state.Name);
        }

        [TestMethod]
        public void GetState_NotFound_ReturnsNull()
        {
            var file = new OnlineRepository(_logger);
            var state = file.GetState("AAA");

            Assert.AreEqual(null, state);
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
