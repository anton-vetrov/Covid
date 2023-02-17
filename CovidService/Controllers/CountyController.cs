using CovidService.Controllers.Exceptions;
using CovidService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidService.Controllers
{
    [ApiController]
    [Route("CovidApi/[controller]")]
    public class CountyController : ControllerBase
    {

        private static int MaximumPageSize = 20;

        private readonly ILogger<CountyController> _logger;
        private ICountyService _countyService;

        public CountyController(ILogger<CountyController> logger, ICountyService countyService)
        {
            _logger = logger;
            _countyService = countyService;

            _logger.LogInformation("_countyService={0}", _countyService);
        }

        [HttpGet("Summary")]
        public PagedCountySummary GetSummary(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            _logger.LogInformation("{}, {}, {}, {}, {}", county, startDate, endDate, pageIndex, pageSize);

            if (endDate < startDate)
            {
                _logger.LogError("Unexpected date range {}-{}", startDate, endDate);
                throw (new UnexpectedInputException());
            }

            if (pageSize == 0)
            {
                _logger.LogError("Unexpected pageSize {}", pageSize);
                throw (new UnexpectedInputException());
            }

            if (pageSize > MaximumPageSize)
            {
                _logger.LogInformation("Page size {} is too big limiting to {}", pageSize, MaximumPageSize);
            }

            // TODO Output date only, currently it outputs date and time
            return _countyService.GetSummary(county, startDate, endDate, pageIndex, pageSize);
        }

        // TODO Breakdown
        // TODO Rate

        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() => Problem();
    }
}
