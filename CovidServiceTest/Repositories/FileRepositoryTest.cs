using CovidService.Models;
using CovidService.Repositories;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace CovidServiceTest.Repositories
{
    [TestClass]
    public class FileRepositoryTest
    {
        [TestMethod]
        public void GetState_ReturnsState()
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                var file = new FileRepository(memoryStream);
                var state = file.GetState("Alabama");

                Assert.AreEqual(69, state.Counties.Count());
                Assert.AreEqual("Alabama", state.Name);
            }
        }

        [TestMethod]
        public void GetState_NotFound_ReturnsNull()
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                var file = new FileRepository(memoryStream);
                var state = file.GetState("AAA");

                Assert.AreEqual(null, state);
            }
        }

        [TestMethod]
        public void GetCounties_ReturnsList()
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                var file = new FileRepository(memoryStream);
                var counties = file.GetCounties();

                Assert.AreEqual(3258, counties.Count());
                Assert.AreEqual("Barbour, Alabama, US", counties[2].CombinedKey);
            }
        }
    }
}
