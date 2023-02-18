using System.Collections.Generic;

namespace CovidService.Services.County
{
    public class PagedCountyRate
    {
        public IEnumerable<CountyRate> CountyRates { get; set; }
        public int TotalPagesCount { get; set; }
    }
}
