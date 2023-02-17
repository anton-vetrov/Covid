using System;
using System.Collections.Generic;

namespace CovidService.Services.County
{
    public class PagedCountySummary
    {
        public IEnumerable<CountySummary> CountySummaries { get; set; }
        public int TotalPagesCount { get; set; }

    }
}
