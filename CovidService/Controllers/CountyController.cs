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
            _logger.LogInformation("GetSummary: {}, {}, {}, {}, {}", county, startDate, endDate, pageIndex, pageSize);

            // TODO Output date only, currently it outputs date and time
            return _countyService.GetSummary(county, startDate, endDate, pageIndex, pageSize);
        }

        // TODO Breakdown
        // TODO Rate
    }
}
