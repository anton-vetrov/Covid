using System;
using System.Collections.Generic;

namespace CovidService.Services.County
{
    public interface ICountyService
    {
        public PagedCountySummary GetSummary(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
        public PagedCountyBreakdown GetBreakdownAndRate(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
    }
}
