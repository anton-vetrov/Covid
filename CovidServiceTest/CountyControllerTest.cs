using CovidService.Controllers;
using CovidService.Controllers.Exceptions;
using CovidService.Repositories;
using CovidService.Services.County;
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
        Mock<ICountyService>  _countyServiceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _countyServiceMock = new Mock<ICountyService>();
            _countyServiceMock.Setup(x => x.GetSummary(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PagedCountySummary());
            _countyServiceMock.Setup(x => x.GetBreakdownAndRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PagedCountyBreakdown());
            _countyServiceMock.Setup(x => x.GetRate(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PagedCountyRate());
        }

        [TestMethod]
        public void GetSummary_Returns()
        {
            var controller = new CountyController(_logger, _countyServiceMock.Object);

            var summary = controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);
            
            Assert.IsNotNull(summary);
            _countyServiceMock.Verify(x => x.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public void GetSummary_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetSummary("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetSummary_InvalidPageSize_Throws()
        {
            

            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetSummary_BlankLocation_Throws()
        {


            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<BlankCountException>(
                () => controller.GetSummary("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetBreakdown_Returns()
        {
            var controller = new CountyController(_logger, _countyServiceMock.Object);

            var summary = controller.GetBreakdown("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);

            Assert.IsNotNull(summary);
            _countyServiceMock.Verify(x => x.GetBreakdownAndRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public void GetBreakdown_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetBreakdown("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetBreakdown_InvalidPageSize_Throws()
        {
            

            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetBreakdown("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetBreakdown_BlankLocation_Throws()
        {


            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<BlankCountException>(
                () => controller.GetBreakdown("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetRate_Returns()
        {
            var controller = new CountyController(_logger, _countyServiceMock.Object);

            var summary = controller.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10);

            Assert.IsNotNull(summary);
            _countyServiceMock.Verify(x => x.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 1, 10), Times.Once());
        }

        [TestMethod]
        public void GetRate_InvalidDateRange_Throws()
        {
            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetRate("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetRate_InvalidPageSize_Throws()
        {


            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetRate("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetRate_BlankLocation_Throws()
        {


            var controller = new CountyController(_logger, _countyServiceMock.Object);

            Assert.ThrowsException<BlankCountException>(
                () => controller.GetRate("", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }
    }
}
