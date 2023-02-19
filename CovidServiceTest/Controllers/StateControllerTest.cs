﻿using CovidService.Controllers;
using CovidService.Controllers.Exceptions;
using CovidService.Services.County;
using CovidService.Services.State;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CovidServiceTest
{
    [TestClass]
    public class StateControllerTest
    {
        ILogger<StateController> _logger = new Mock<ILogger<StateController>>().Object;
        Mock<IStateService> _serviceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _serviceMock = new Mock<IStateService>();
            _serviceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new StateSummary());
            /*
            _countyServiceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PagedCountyBreakdown());
            _countyServiceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PagedCountyRate());
            */
        }

        [TestMethod]
        public void GetSummary_Returns()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            _serviceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new StateSummary() { State = "Alabama" });

            var summary = controller.GetSummary("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01));

            Assert.IsNotNull(summary);
            Assert.AreEqual("Alabama", summary.State);
            _serviceMock.Verify(x => x.GetSummary("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01)), Times.Once());
        }

        [TestMethod]
        public void GetSummary_BlankState_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<BlankStateException>(
                () => controller.GetSummary("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public void GetSummary_InvalidDateRange_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetSummary("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public void GetSummary_InvalidStateName_Throws()
        {
            _serviceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((StateSummary)null);

            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<StateNotFoundException>(
                () => controller.GetSummary("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

    }
}
