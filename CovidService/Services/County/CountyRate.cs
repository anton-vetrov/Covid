using System.Collections.Generic;

namespace CovidService.Services.County
{
    public class CountyRate
    {
        public IEnumerable<DateRate> DateRates { get; set; }
        public string County { get; set; }
    }
}
