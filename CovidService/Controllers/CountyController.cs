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

            _logger.LogInformation("CountyController: _countyService={0}", _countyService);
        }

        [HttpGet("Summary")]
        public List<CountySummary> GetSummary(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            _logger.LogInformation("GetSummary: {}, {}, {}, {}, {}", county, startDate, endDate, pageIndex, pageSize);

            /*
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            */
            return _countyService.GetSummary(county, startDate, endDate, pageIndex, pageSize).ToList();
        }

        // TODO Breakdown
        // TODO Rate
    }
}
