using System.Collections.Generic;

namespace CovidService.Services
{
    public class CountyBreakdown
    {
        public IEnumerable<DateBreakdown> DateBreakdowns { get; set; }
        public string County { get; set; }
    }
}
