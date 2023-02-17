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
        public void GetSummary_EmptyName_ReturnsAllTheCounties()
        {
            var summaries = _countyService.GetSummary("", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 0, 100000);

            Assert.AreEqual(3258, summaries.CountySummary.Count());
            Assert.AreEqual(3258, summaries.TotalPagesCount);
        }

        [TestMethod]
        public void GetSummary_EmptyName_ReturnsSecondPage()
        {
            var summaries = _countyService.GetSummary("", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 1, 10);

            Assert.AreEqual(10, summaries.CountySummary.Count());
            Assert.AreEqual(3258, summaries.TotalPagesCount);
            var county = summaries.CountySummary.First();
            Assert.AreEqual("Chilton, Alabama, US", county.County);
            Assert.AreEqual(12876, county.Cases.Minimum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 1), county.Cases.Minimum.Date);
            Assert.AreEqual(12905, county.Cases.Maximum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 8), county.Cases.Maximum.Date);
        }


        [TestMethod]
        public void GetSummary_ValidName_ReturnsFirstPage()
        {
            var summaries = _countyService.GetSummary("Harris", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 1, 1);

            Assert.AreEqual(1, summaries.CountySummary.Count());
            Assert.AreEqual(2, summaries.TotalPagesCount);
            var county = summaries.CountySummary.First();
            Assert.AreEqual("Harris, Texas, US", county.County);
            Assert.AreEqual(1262246, county.Cases.Minimum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 1), county.Cases.Minimum.Date);
            Assert.AreEqual(1265347, county.Cases.Maximum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 9), county.Cases.Maximum.Date);
        }

        [TestMethod]
        public void GetSummary_InvalidPageIndex_ReturnsEmptyList()
        {
            var summaries = _countyService.GetSummary("Harris", new DateTime(2023, 02, 21), new DateTime(2023, 02, 13), 1, 1);

            Assert.AreEqual(0, summaries.CountySummary.Count());
            Assert.AreEqual(2, summaries.TotalPagesCount);
        }


        [TestMethod]
        public void GetSummary_CountyNotFound_ReturnsEmptyList()
        {
            var summaries = _countyService.GetSummary("AAA", new DateTime(2023, 02, 21), new DateTime(2023, 02, 13), 1, 1);

            Assert.AreEqual(0, summaries.CountySummary.Count());
            Assert.AreEqual(0, summaries.TotalPagesCount);
        }

        [TestMethod]
        public void GetSummary_InvalidPaging_ReturnsEmptyList()
        {
            var summaries = _countyService.GetSummary("AAA", new DateTime(2023, 02, 21), new DateTime(2023, 02, 13), -1, 0);

            Assert.AreEqual(0, summaries.CountySummary.Count());
            Assert.AreEqual(0, summaries.TotalPagesCount);
        }
    }
}
