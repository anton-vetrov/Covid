using System.Collections.Generic;

namespace CovidService.Services
{
    public class PagedCountyBreakdown
    {
        public IEnumerable<CountyBreakdown> CountyBreakdowns { get; set; }
        public int TotalPagesCount { get; set; }

    }
}
