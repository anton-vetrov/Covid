using CovidService.Controllers;
using CovidService.Controllers.Exceptions;
using CovidService.Services.County;
using CovidService.Services.County.Extensions;
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
            _serviceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new StateBreakdown());
            _serviceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new StateRate());
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

            Assert.ThrowsException<UnexpectedDateRangeException>(
                () => controller.GetSummary("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public void GetSummary_BlankStartDate_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedDateRangeException>(
                () => controller.GetSummary("Test", StatExtension._blankDateTime, new DateTime(2023, 01, 01))
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

        [TestMethod]
        public void GetBreakdown_Returns()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            _serviceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new StateBreakdown() { State = "Alabama" });

            var breakdown = controller.GetBreakdown("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01));

            Assert.IsNotNull(breakdown);
            Assert.AreEqual("Alabama", breakdown.State);
            _serviceMock.Verify(x => x.GetBreakdownAndRate("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01)), Times.Once());
        }

        [TestMethod]
        public void GetBreakdown_BlankState_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<BlankStateException>(
                () => controller.GetBreakdown("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public void GetBreakdown_InvalidDateRange_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedDateRangeException>(
                () => controller.GetBreakdown("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public void GetBreakdown_BlankStartDate_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedDateRangeException>(
                () => controller.GetBreakdown("Test", StatExtension._blankDateTime, new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public void GetBreakdown_InvalidStateName_Throws()
        {
            _serviceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((StateBreakdown)null);

            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<StateNotFoundException>(
                () => controller.GetBreakdown("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public void GetRate_Returns()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            _serviceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new StateRate() { State = "Alabama" });

            var breakdown = controller.GetRate("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01));

            Assert.IsNotNull(breakdown);
            Assert.AreEqual("Alabama", breakdown.State);
            _serviceMock.Verify(x => x.GetRate("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01)), Times.Once());
        }

        [TestMethod]
        public void GetRate_BlankState_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<BlankStateException>(
                () => controller.GetRate("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public void GetRate_InvalidDateRange_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedDateRangeException>(
                () => controller.GetRate("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public void GetRate_BlankStartDate_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedDateRangeException>(
                () => controller.GetRate("Test", StatExtension._blankDateTime, new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public void GetRate_InvalidStateName_Throws()
        {
            _serviceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((StateRate)null);

            var controller = new StateController(_logger, _serviceMock.Object);

            Assert.ThrowsException<StateNotFoundException>(
                () => controller.GetRate("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }
    }
}
