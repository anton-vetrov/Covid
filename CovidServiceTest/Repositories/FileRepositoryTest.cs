using CovidService.Models;
using CovidService.Repositories;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace CovidServiceTest.Repositories
{
    [TestClass]
    public class FileRepositoryTest
    {
        [TestMethod]
        public async Task GetStateAsync_ReturnsState()
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                var file = new FileRepository(memoryStream);
                var state = await file.GetStateAsync("Alabama");

                Assert.AreEqual(69, state.Counties.Count());
                Assert.AreEqual("Alabama", state.Name);
            }
        }

        [TestMethod]
        public async Task GetStateAsync_NotFound_ReturnsNull()
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                var file = new FileRepository(memoryStream);
                var state = await file.GetStateAsync("AAA");

                Assert.AreEqual(null, state);
            }
        }

        [TestMethod]
        public async Task GetCountiesAsync_ReturnsList()
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                var file = new FileRepository(memoryStream);
                var counties = await file.GetCountiesAsync();

                Assert.AreEqual(3258, counties.Count());
                Assert.AreEqual("Barbour, Alabama, US", counties[2].CombinedKey);
            }
        }
    }
}
