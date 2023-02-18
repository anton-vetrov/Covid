using System.Collections.Generic;

namespace CovidService.Services.County
{
    public class CountyRate
    {
        public IEnumerable<DateRate> DateBreakdowns { get; set; }
        public string County { get; set; }
    }
}
