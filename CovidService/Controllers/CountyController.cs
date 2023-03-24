using CovidService.Controllers.Exceptions;
using CovidService.Services.County;
using CovidService.Services.County.Extensions;
using DocumentFormat.OpenXml.Wordprocessing;
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

        /// <summary>
        /// Throws UnexpectedInputException
        /// </summary>
        /// <param name="county"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Summary")]
        public async Task<PagedCountySummary> GetSummary(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            _logger.LogInformation("GetSummary={0}", county);

            ValidateInput(county, startDate, endDate, pageIndex, pageSize);

            // TODO Output date only, currently it outputs date and time
            var summary = await _countyService.GetSummary(county, startDate, endDate, pageIndex, pageSize);
            if (summary.TotalPagesCount == 0)
                throw (new CountyNotFoundException());

            return summary;
        }

        [HttpGet("Breakdown")]
        public async Task<PagedCountyBreakdown> GetBreakdown(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize) 
        {
            ValidateInput(county, startDate, endDate, pageIndex, pageSize);

            return await _countyService.GetBreakdownAndRate(county, startDate, endDate, pageIndex, pageSize);
        }

        [HttpGet("Rate")]
        public async Task<PagedCountyRate> GetRate(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            ValidateInput(county, startDate, endDate, pageIndex, pageSize);

            return await _countyService.GetRate(county, startDate, endDate, pageIndex, pageSize);
        }

        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() => Problem();

        private void ValidateInput(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            if (String.IsNullOrEmpty(county))
            {
                _logger.LogError("The location is blank");
                throw (new BlankCountyException());
            }

            if (endDate < startDate || startDate == CountyExtensions._blankDateTime)
            {
                _logger.LogError("Unexpected date range {}-{}", startDate, endDate);
                throw (new UnexpectedDateRangeException());
            }

            if (pageSize == 0)
            {
                _logger.LogError("Unexpected pageSize {}", pageSize);
                throw (new UnexpectedDateRangeException());
            }

            if (pageSize > MaximumPageSize)
            {
                _logger.LogInformation("Page size {} is too big limiting to {}", pageSize, MaximumPageSize);
            }

            //_logger.LogInformation("{}, {}, {}, {}, {}", county, startDate, endDate, pageIndex, pageSize);
        }
    }
}
