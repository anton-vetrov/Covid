using System.Collections.Generic;

namespace CovidService.Services.County
{
    public class CountyBreakdown
    {
        public IEnumerable<DateBreakdown> DateBreakdowns { get; set; }
        public string County { get; set; }
    }
}
