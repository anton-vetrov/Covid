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
                .Returns(new PagedCountySummary());
            _serviceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PagedCountyBreakdown());
            _serviceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PagedCountyRate());
        }

        [TestMethod]
        public void GetSummary_Returns()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            _serviceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PagedCountySummary() { TotalPagesCount = 1 } );

            var summary = controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);
            
            Assert.IsNotNull(summary);
            _serviceMock.Verify(x => x.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public void GetSummary_InvalidCountyName_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<CountyNotFoundException>(
                () => controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10)
            );
        }

        [TestMethod]
        public void GetSummary_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetSummary("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetSummary_InvalidPageSize_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetSummary_BlankCounty_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<BlankCountyException>(
                () => controller.GetSummary("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetBreakdown_Returns()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            var summary = controller.GetBreakdown("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);

            Assert.IsNotNull(summary);
            _serviceMock.Verify(x => x.GetBreakdownAndRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public void GetBreakdown_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetBreakdown("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetSummary_BlankStartDate_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetBreakdown("Test", StatExtension._blankDateTime, new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetBreakdown_InvalidPageSize_Throws()
        {
            

            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetBreakdown("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetBreakdown_BlankLocation_Throws()
        {


            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<BlankCountyException>(
                () => controller.GetBreakdown("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetRate_Returns()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            var summary = controller.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);

            Assert.IsNotNull(summary);
            _serviceMock.Verify(x => x.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public void GetRate_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetRate("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetRate_InvalidPageSize_Throws()
        {


            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetRate_BlankLocation_Throws()
        {


            var controller = new CountyController(_logger, _serviceMock.Object);

            Assert.ThrowsException<BlankCountyException>(
                () => controller.GetRate("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }
    }
}
