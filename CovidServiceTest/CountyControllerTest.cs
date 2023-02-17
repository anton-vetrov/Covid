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

        [TestMethod]
        public void GetSummary_InvalidDateRange_Throws()
        {
            var countyServiceMock = new Mock<ICountyService>();
            
            var controller = new CountyController(_logger, countyServiceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetSummary("Test", new DateTime(2023, 02, 01), new DateTime(2023, 01, 01), 0, 0)
            );
        }

        [TestMethod]
        public void GetSummary_InvalidPageSize_Throws()
        {
            var countyServiceMock = new Mock<ICountyService>();

            var controller = new CountyController(_logger, countyServiceMock.Object);

            Assert.ThrowsException<UnexpectedInputException>(
                () => controller.GetSummary("Test", new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 0, 0)
            );
        }
    }
}
