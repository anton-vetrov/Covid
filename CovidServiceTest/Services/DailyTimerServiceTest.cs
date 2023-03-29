using CovidService.Services.Github;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidServiceTest.Services
{
    [TestClass]
    public class DailyTimerServiceTest
    {
        private DailyTimerService _service;

        [TestInitialize]
        public void TestInitialize()
        {
        }


        [TestMethod]
        public void IsNewDay_DayChange_Returns()
        {
            var currentTime = DateTime.Now;
            var theDayBefore = currentTime.AddDays(-1);
            _service = new DailyTimerService(theDayBefore.Ticks);

            var isNewDay = _service.IsNewDay;

            Assert.AreEqual(true, isNewDay);
        }

        [TestMethod]
        public void IsNewDay_SameDay_Returns()
        {
            var currentTime = DateTime.Now;
            _service = new DailyTimerService(currentTime.Ticks);

            var isNewDay = _service.IsNewDay;

            Assert.AreEqual(false, isNewDay);
        }
    }
}
