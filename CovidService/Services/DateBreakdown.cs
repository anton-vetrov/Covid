using System;

namespace CovidService.Services
{
    public class DateBreakdown
    {
        public DateTime Date { get; set; }
        public int NewCases { get; set; }
        public int TotalCases { get; set; }
        public double RatePercentage { get; set; }
    }
}
