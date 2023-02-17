using CovidService.Repositories;
using CovidService.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Namotion.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CovidServiceTest
{
    [TestClass]
    public class CountyServiceTest
    {
        private static FileRepository _fileRepository;

        private CountyService _countyService;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                _fileRepository = new FileRepository(memoryStream);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _countyService = new CountyService(_fileRepository);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }


        [TestMethod]
        public void GetSummary_ReturnsList()
        {
            var summaries = _countyService.GetSummary("Harris", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 1, 1);

            Assert.AreEqual(1, summaries.Count());
            var county = summaries.First();
            Assert.AreEqual("Harris, Texas, US", county.County);
            Assert.AreEqual(1262246, county.Cases.Minimum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 1), county.Cases.Minimum.Date);
            Assert.AreEqual(1265347, county.Cases.Maximum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 9), county.Cases.Maximum.Date);
        }

    }
}
