using CovidService.Controllers;
using CovidService.Controllers.Exceptions;
using CovidService.Repositories;
using CovidService.Services.County;
using CovidService.Services.County.Extensions;
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
    public class CountyControllerTest
    {
        ILogger<CountyController> _logger = new Mock<ILogger<CountyController>>().Object;
        Mock<ICountyService>  _serviceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _serviceMock = new Mock<ICountyService>();
            _serviceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Extensions.NewTask<PagedCountySummary>());
            _serviceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Extensions.NewTask<PagedCountyBreakdown>());
            _serviceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Extensions.NewTask <PagedCountyRate>());
        }

        [TestMethod]
        public async Task GetSummary_Returns()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            _serviceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Extensions.NewTask<PagedCountySummary>((summary) => { summary.TotalPagesCount = 1; }));

            var summary = await controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);
            
            Assert.IsNotNull(summary);
            _serviceMock.Verify(x => x.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public async Task GetSummary_InvalidCountyName_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<CountyNotFoundException>(
                () => controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10)
            );
        }

        [TestMethod]
        public async Task GetSummary_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetSummary("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetSummary_InvalidPageSize_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetSummary_BlankCounty_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<BlankCountyException>(
                () => controller.GetSummary("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetBreakdown_Returns()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            var summary = await controller.GetBreakdown("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);

            Assert.IsNotNull(summary);
            _serviceMock.Verify(x => x.GetBreakdownAndRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public async Task GetBreakdown_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetBreakdown("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetSummary_BlankStartDate_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetBreakdown("Test", StatExtension._blankDateTime, new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetBreakdown_InvalidPageSize_Throws()
        {
            

            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetBreakdown("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetBreakdown_BlankLocation_Throws()
        {


            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<BlankCountyException>(
                () => controller.GetBreakdown("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetRate_Returns()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            var summary = await controller.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);

            Assert.IsNotNull(summary);
            _serviceMock.Verify(x => x.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public async Task GetRate_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetRate("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetRate_InvalidPageSize_Throws()
        {


            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<UnexpectedDateRangeException>(
                () => controller.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public async Task GetRate_BlankLocation_Throws()
        {


            var controller = new CountyController(_logger, _serviceMock.Object);

            await Assert.ThrowsExceptionAsync<BlankCountyException>(
                () => controller.GetRate("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }
    }
}
