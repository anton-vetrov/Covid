using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidService.Services.County
{
    public interface ICountyService
    {
        Task<PagedCountySummary> GetSummary(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
        Task<PagedCountyBreakdown> GetBreakdownAndRate(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
        Task<PagedCountyRate> GetRate(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
    }
}
