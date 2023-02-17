using CovidService.Models;
using CovidService.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace CovidServiceTest
{
    [TestClass]
    public class TestRepositoryTest
    {
        [TestMethod]
        public void GetCounties_ReturnsList()
        {
            var test = new TestRepository();
            var counties = test.GetCounties();

            Assert.AreEqual(2, counties.Count());
            Assert.AreEqual("Autauga", counties[0].Name);
        }

    }
}
