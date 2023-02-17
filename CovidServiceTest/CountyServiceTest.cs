using CovidService.Repositories;
using CovidService.Services.County;
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

            Assert.AreEqual(3258, summaries.CountySummaries.Count());
            Assert.AreEqual(3258, summaries.TotalPagesCount);
        }

        [TestMethod]
        public void GetSummary_EmptyName_ReturnsSecondPage()
        {
            var summaries = _countyService.GetSummary("", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 1, 10);

            Assert.AreEqual(10, summaries.CountySummaries.Count());
            Assert.AreEqual(3258, summaries.TotalPagesCount);
            var county = summaries.CountySummaries.First();
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

            Assert.AreEqual(1, summaries.CountySummaries.Count());
            Assert.AreEqual(2, summaries.TotalPagesCount);
            var county = summaries.CountySummaries.First();
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

            Assert.AreEqual(0, summaries.CountySummaries.Count());
            Assert.AreEqual(2, summaries.TotalPagesCount);
        }


        [TestMethod]
        public void GetSummary_CountyNotFound_ReturnsEmptyList()
        {
            var summaries = _countyService.GetSummary("AAA", new DateTime(2023, 02, 21), new DateTime(2023, 02, 13), 1, 1);

            Assert.AreEqual(0, summaries.CountySummaries.Count());
            Assert.AreEqual(0, summaries.TotalPagesCount);
        }

        [TestMethod]
        public void GetSummary_InvalidPaging_ReturnsEmptyList()
        {
            var summaries = _countyService.GetSummary("AAA", new DateTime(2023, 02, 21), new DateTime(2023, 02, 13), -1, 0);

            Assert.AreEqual(0, summaries.CountySummaries.Count());
            Assert.AreEqual(0, summaries.TotalPagesCount);
        }

        [TestMethod]
        public void GetBreakDown_EmptyName_ReturnsAllTheCounties()
        {
            var breakdowns = _countyService.GetBreakdown("", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 0, 100000);

            Assert.AreEqual(3258, breakdowns.CountyBreakdowns.Count());
            Assert.AreEqual(3258, breakdowns.TotalPagesCount);
            var countyBreakdowns = breakdowns.CountyBreakdowns.ToArray();
            Assert.AreEqual(12, countyBreakdowns[0].DateBreakdowns.Count());
            Assert.AreEqual("Autauga, Alabama, US", countyBreakdowns[0].County);
            var breakdown = countyBreakdowns[0].DateBreakdowns.ToArray()[6];
            Assert.AreEqual(new DateTime(2023, 2, 8), breakdown.Date);
            Assert.AreEqual(19630, breakdown.TotalCases);
            Assert.AreEqual(19630 - 19530, breakdown.NewCases);
        }

        [TestMethod]
        public void GetBreakDown_EmptyName_ReturnsSecondPage()
        {
            var breakdowns = _countyService.GetBreakdown("", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 1, 10);

            Assert.AreEqual(10, breakdowns.CountyBreakdowns.Count());
            Assert.AreEqual(3258, breakdowns.TotalPagesCount);
            var county = breakdowns.CountyBreakdowns.First();
            Assert.AreEqual("Chilton, Alabama, US", county.County);
            var breakdown = county.DateBreakdowns.ToArray()[6];
            Assert.AreEqual(new DateTime(2023, 2, 8), breakdown.Date);
            Assert.AreEqual(12905, breakdown.TotalCases);
            Assert.AreEqual(12905 - 12876, breakdown.NewCases);
        }


        [TestMethod]
        public void GetBreakDown_ValidName_ReturnsFirstPage()
        {
            var breakdowns = _countyService.GetBreakdown("Harris", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13), 1, 1);

            Assert.AreEqual(1, breakdowns.CountyBreakdowns.Count());
            Assert.AreEqual(2, breakdowns.TotalPagesCount);
            var county = breakdowns.CountyBreakdowns.First();
            Assert.AreEqual("Harris, Texas, US", county.County);
            var breakdown = county.DateBreakdowns.ToArray()[7];
            Assert.AreEqual(new DateTime(2023, 2, 9), breakdown.Date);
            Assert.AreEqual(1265347, breakdown.TotalCases);
            Assert.AreEqual(1265347 - 1262246, breakdown.NewCases);
        }

        [TestMethod]
        public void GetBreakDown_InvalidPageIndex_ReturnsEmptyList()
        {
            var breakdowns = _countyService.GetBreakdown("Harris", new DateTime(2023, 02, 21), new DateTime(2023, 02, 13), 1, 1);

            Assert.AreEqual(0, breakdowns.CountyBreakdowns.Count());
            Assert.AreEqual(2, breakdowns.TotalPagesCount);
        }


        [TestMethod]
        public void GetBreakDown_CountyNotFound_ReturnsEmptyList()
        {
            var breakdowns = _countyService.GetBreakdown("AAA", new DateTime(2023, 02, 21), new DateTime(2023, 02, 13), 1, 1);

            Assert.AreEqual(0, breakdowns.CountyBreakdowns.Count());
            Assert.AreEqual(0, breakdowns.TotalPagesCount);
        }

        [TestMethod]
        public void GetBreakDown_InvalidPaging_ReturnsEmptyList()
        {
            var breakdowns = _countyService.GetBreakdown("AAA", new DateTime(2023, 02, 21), new DateTime(2023, 02, 13), -1, 0);

            Assert.AreEqual(0, breakdowns.CountyBreakdowns.Count());
            Assert.AreEqual(0, breakdowns.TotalPagesCount);
        }
    }
}
