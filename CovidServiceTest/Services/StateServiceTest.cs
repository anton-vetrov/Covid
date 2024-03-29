﻿using CovidService.Repositories;
using CovidService.Services.County;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using CovidService.Services.County.Extensions;
using CovidService.Models;
using CovidService.Services.State;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using CovidService.Services.Exceptions;

namespace CovidServiceTest.Services
{

    [TestClass]
    public class StateServiceTest
    {
        private static FileRepository _fileRepository;

        private StateService _stateService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                _fileRepository = new FileRepository(memoryStream);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _stateService = new StateService(_fileRepository);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }


        [TestMethod]
        public async Task GetSummary_ValidStateName_Returns()
        {
            var summary = await _stateService.GetSummary("Alabama", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13));

            Assert.AreEqual("Alabama", summary.State);
            Assert.AreEqual(23512.792753623184, summary.Cases.Average);
            Assert.AreEqual(0, summary.Cases.Minimum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 1), summary.Cases.Minimum.Date);
            Assert.AreEqual(235948, summary.Cases.Maximum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 8), summary.Cases.Maximum.Date);
        }

        [TestMethod]
        public async Task GetSummary_ValidStateNameBlankRange_ReturnsAll()
        {
            var summary = await _stateService.GetSummary("Alabama", CountyExtensions._blankDateTime, CountyExtensions._blankDateTime);

            Assert.AreEqual("Alabama", summary.State);
            Assert.AreEqual(23246.166666666664, summary.Cases.Average);
            Assert.AreEqual(0, summary.Cases.Minimum.Count);
            Assert.AreEqual(new DateTime(2023, 1, 1), summary.Cases.Minimum.Date);
            Assert.AreEqual(235948, summary.Cases.Maximum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 8), summary.Cases.Maximum.Date);
        }

        [TestMethod]
        public async Task GetSummary_InvalidStateName_ReturnsNull()
        {
            var summary = await _stateService.GetSummary("AAA", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13));

            Assert.AreEqual(null, summary);
        }

        [TestMethod]
        public async Task GetBreakdownAndRate_ValidStateName_Returns()
        {
            var stateBreakdown = await _stateService.GetBreakdownAndRate("Alabama", new DateTime(2023, 02, 07), new DateTime(2023, 02, 13));

            Assert.AreEqual("Alabama", stateBreakdown.State);
            var breakdown = stateBreakdown.DateBreakdowns.First();
            Assert.AreEqual(new DateTime(2023, 02, 07).AddDays(1), breakdown.Date);
            Assert.AreEqual(1627670, breakdown.TotalCases);
            Assert.AreEqual(1627670 - 1617850, breakdown.NewCases);
            Assert.AreEqual((1627670.0 - 1617850.0) * 100.0 / 1627670.0, breakdown.RatePercentage);
        }

        [TestMethod]
        public async Task GetBreakdownAndRate_ValidStateNameBlankRange_ReturnsAll()
        {
            var stateBreakdown = await _stateService.GetBreakdownAndRate("Alabama", CountyExtensions._blankDateTime, CountyExtensions._blankDateTime);

            Assert.AreEqual("Alabama", stateBreakdown.State);
            var breakdown = stateBreakdown.DateBreakdowns.First();
            Assert.AreEqual(new DateTime(2023, 1, 1).AddDays(1), breakdown.Date);
            Assert.AreEqual(1568934, breakdown.TotalCases);
            Assert.AreEqual(0, breakdown.NewCases);
            Assert.AreEqual(0, breakdown.RatePercentage);

            var lastBreakdown = stateBreakdown.DateBreakdowns.Last();
            Assert.AreEqual(new DateTime(2023, 2, 13), lastBreakdown.Date);
            Assert.AreEqual(1627670, lastBreakdown.TotalCases);
            Assert.AreEqual(0, lastBreakdown.NewCases);
            Assert.AreEqual(0, lastBreakdown.RatePercentage);
        }

        [TestMethod]
        public async Task GetBreakdownAndRate_InvalidStateName_ReturnsNull()
        {
            var stateBreakdown = await _stateService.GetBreakdownAndRate("AAA", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13));

            Assert.AreEqual(null, stateBreakdown);
        }

        [TestMethod]
        public async Task GetBreakdownAndRate_NoDataAvailable_Throws()
        {
            await Assert.ThrowsExceptionAsync<EmptyResultException>(
                () => _stateService.GetBreakdownAndRate("California", new DateTime(2023, 03, 21), new DateTime(2023, 03, 31))
            );

        }

        [TestMethod]
        public async Task GetRate_ValidStateName_Returns()
        {
            var rate = await _stateService.GetRate("Alabama", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13));

            Assert.AreEqual(12, rate.DateRates.Count());
            Assert.AreEqual("Alabama", rate.State);
            var dateRate = rate.DateRates.ToArray()[6];
            Assert.AreEqual(new DateTime(2023, 2, 8), dateRate.Date);
            Assert.AreEqual((1627670.0 - 1617850.0) * 100.0 / 1627670.0, dateRate.Percentage);
        }

        // TODO More tests for GetRate, even though it is uses GetBreakdownAndRate under the hood

        public class Record
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}
