using System;

namespace CovidService.Services
{
    public class Breakdown
    {
        public DateTime Date { get; set; }
        public int NewCases { get; set; }
        public int TotalCases { get; set; }
    }
}
