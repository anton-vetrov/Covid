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
using System.Threading.Tasks;

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
                .Returns(Extensions.NewTask <StateSummary>());
            _serviceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Extensions.NewTask <StateBreakdown>());
            _serviceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Extensions.NewTask <StateRate>());
        }

        [TestMethod]
        public async Task GetSummary_Returns()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            _serviceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Extensions.NewTask <StateSummary>((summary) => { summary.State = "Alabama"; }));

            var summary = await controller.GetSummary("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01));

            Assert.IsNotNull(summary);
            Assert.AreEqual("Alabama", summary.State);
            _serviceMock.Verify(x => x.GetSummary("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01)), Times.Once());
        }

        [TestMethod]
        public async Task GetSummary_BlankState_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<BlankStateException>(
                () => controller.GetSummary("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public async Task GetSummary_InvalidDateRange_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetSummary("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public async Task GetSummary_BlankStartDate_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetSummary("Test", CountyExtensions._blankDateTime, new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public async Task GetSummary_InvalidStateName_Throws()
        {
            _serviceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<StateSummary>(null));

            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<StateNotFoundException>(
                () => controller.GetSummary("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public async Task GetBreakdown_Returns()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            _serviceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Extensions.NewTask <StateBreakdown>((breakdown) => { breakdown.State = "Alabama"; }));

            var breakdown = await controller.GetBreakdown("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01));

            Assert.IsNotNull(breakdown);
            Assert.AreEqual("Alabama", breakdown.State);
            _serviceMock.Verify(x => x.GetBreakdownAndRate("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01)), Times.Once());
        }

        [TestMethod]
        public async Task GetBreakdown_BlankState_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<BlankStateException>(
                () => controller.GetBreakdown("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public async Task GetBreakdown_InvalidDateRange_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetBreakdown("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public async Task GetBreakdown_BlankStartDate_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetBreakdown("Test", CountyExtensions._blankDateTime, new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public async Task GetBreakdown_InvalidStateName_Throws()
        {
            _serviceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<StateBreakdown>(null));

            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<StateNotFoundException>(
                () => controller.GetBreakdown("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public async Task GetRate_Returns()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            _serviceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Extensions.NewTask<StateRate>((rate) => { rate.State = "Alabama";  }));

            var breakdown = await controller.GetRate("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01));

            Assert.IsNotNull(breakdown);
            Assert.AreEqual("Alabama", breakdown.State);
            _serviceMock.Verify(x => x.GetRate("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01)), Times.Once());
        }

        [TestMethod]
        public async Task GetRate_BlankState_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<BlankStateException>(
                () => controller.GetRate("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }

        [TestMethod]
        public async Task GetRate_InvalidDateRange_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetRate("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public async Task GetRate_BlankStartDate_Throws()
        {
            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetRate("Test", CountyExtensions._blankDateTime, new DateTime(2023, 01, 01))
            );
        }

        [TestMethod]
        public async Task GetRate_InvalidStateName_Throws()
        {
            _serviceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<StateRate>(null));

            var controller = new StateController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<StateNotFoundException>(
                () => controller.GetRate("Alabama", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01))
            );
        }
    }
}
