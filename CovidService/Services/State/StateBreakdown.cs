using System.Collections.Generic;

namespace CovidService.Services.State
{
    public class StateBreakdown
    {
        public IEnumerable<DateBreakdown> DateBreakdowns { get; set; }
        public string State { get; set; }
    }
}
