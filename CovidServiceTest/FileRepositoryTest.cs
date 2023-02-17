using CovidService.Models;
using CovidService.Repositories;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace CovidServiceTest
{
    [TestClass]
    public class FileRepositoryTest
    {
        [TestMethod]
        public void GetCounties_ReturnsList()
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                var file = new FileRepository(memoryStream);
                var counties = file.GetCounties();

                Assert.AreEqual(2, counties.Count());
                Assert.AreEqual("Autauga", counties[0].Name);
            }
        }

    }
}
