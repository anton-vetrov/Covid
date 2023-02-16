using CovidService.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CovidServiceTest
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void GetCounties_TestRepository_ReturnsList()
        {
            var test = new TestRepository();
            var counties = test.GetCounties();

            Assert.AreEqual(2, counties.Count());
            Assert.AreEqual("Autauga",  counties[0].Name);
        }
    }
}
