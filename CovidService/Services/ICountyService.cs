using System;
using System.Collections.Generic;

namespace CovidService.Services
{
    public interface ICountyService
    {
        public IEnumerable<CountySummary> GetSummary(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
        public Breakdown GetBreakDown();
        public Rate GetRate();
    }
}
