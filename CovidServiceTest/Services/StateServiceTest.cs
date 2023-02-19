using CovidService.Repositories;
using CovidService.Services.County;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using CovidService.Services.County.Extensions;

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
        public void GetSummary_ValidStateName_Returns()
        {
            var summary = _stateService.GetSummary("Alabama", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13));

            Assert.AreEqual("Alabama", summary.State);
            Assert.AreEqual(23512.792753623184, summary.Cases.Average);
            Assert.AreEqual(0, summary.Cases.Minimum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 1), summary.Cases.Minimum.Date);
            Assert.AreEqual(235948, summary.Cases.Maximum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 8), summary.Cases.Maximum.Date);
        }

        [TestMethod]
        public void GetSummary_ValidStateNameBlankRange_ReturnsAll()
        {
            var summary = _stateService.GetSummary("Alabama", StatExtension._blankDateTime, StatExtension._blankDateTime);

            Assert.AreEqual("Alabama", summary.State);
            Assert.AreEqual(10794.324637681162, summary.Cases.Average);
            Assert.AreEqual(0, summary.Cases.Minimum.Count);
            Assert.AreEqual(new DateTime(2020, 1, 22), summary.Cases.Minimum.Date);
            Assert.AreEqual(235948, summary.Cases.Maximum.Count);
            Assert.AreEqual(new DateTime(2023, 2, 8), summary.Cases.Maximum.Date);
        }

        [TestMethod]
        public void GetSummary_InvalidStateName_ReturnsNull()
        {
            var summary = _stateService.GetSummary("AAA", new DateTime(2023, 02, 01), new DateTime(2023, 02, 13));

            Assert.AreEqual(null, summary);
        }
    }

}
