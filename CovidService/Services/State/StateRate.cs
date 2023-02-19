using System.Collections.Generic;

namespace CovidService.Services.State
{
    public class StateRate
    {
        public IEnumerable<DateRate> DateRates { get; set; }
        public string State { get; set; }
    }
}
