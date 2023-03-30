using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidService.Repositories;
using CovidService.Services.County.Extensions;
using CovidService.Services.Exceptions;

namespace CovidService.Services.County
{
    public class CountyService : ICountyService
    {
        private IRepository _repository;
        public CountyService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<PagedCountySummary> GetSummary(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            IEnumerable<CovidService.Models.County> counties = await _repository.GetCountiesAsync();

            if (!string.IsNullOrEmpty(countyName))
                counties = counties.Where(x => x.Name == countyName);

            var totalPagesCount = counties.Count();
            counties = counties.Skip(pageIndex * pageSize).Take(pageSize);

            return new PagedCountySummary()
            {
                CountySummaries = counties.Summary(startDate, endDate),
                TotalPagesCount = totalPagesCount
            };
        }
        public async Task<PagedCountyBreakdown> GetBreakdownAndRate(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            var countyBreakdowns = new List<CountyBreakdown>();

            IEnumerable<CovidService.Models.County> counties = await _repository.GetCountiesAsync();

            if (!string.IsNullOrEmpty(countyName))
                counties = counties.Where(x => x.Name == countyName);

            var paged = new PagedCountyBreakdown()
            {
                CountyBreakdowns = countyBreakdowns,
                TotalPagesCount = counties.Count()
            };

            counties = counties.Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var county in counties)
            {
                var cases = (startDate == CountyExtensions._blankDateTime && endDate == CountyExtensions._blankDateTime)
                    ? county.Cases
                    : county.Cases.Where(x => x.Key >= startDate && x.Key <= endDate);
                if (cases.Count() == 0)
                {
                    if (county.Cases.Count > 0)
                    {
                        var lastAvailableDate = county.Cases.Last().Key;
                        if (lastAvailableDate < startDate)
                            throw new EmptyResultException(lastAvailableDate);
                    }
                    break;
                }

                var breakdown = cases.Zip(
                    cases.Skip(1),
                    (x, y) => new DateBreakdown()
                    {
                        NewCases = y.Value.Count - x.Value.Count,
                        TotalCases = y.Value.Count,
                        RatePercentage = (x.Value.Count == 0)? 0: ((double)(y.Value.Count - x.Value.Count)) * 100.00 / ((double)x.Value.Count),
                        Date = y.Key
                    }
                );

                countyBreakdowns.Add(new CountyBreakdown()
                {
                    County = county.CombinedKey,
                    DateBreakdowns = breakdown
                });
            }

            return paged;
        }

        public async Task<PagedCountyRate> GetRate(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            var breakdown = await GetBreakdownAndRate(countyName, startDate, endDate, pageIndex, pageSize);

            var paged = new PagedCountyRate()
            {
                CountyRates = breakdown.CountyBreakdowns
                    .Select(x => new CountyRate() {
                        County = x.County,
                        DateRates = x.DateBreakdowns.Select(dateBreakdown => new DateRate() {
                            Date = dateBreakdown.Date,
                            Percentage = dateBreakdown.RatePercentage
                        })
                    }),
                TotalPagesCount = breakdown.TotalPagesCount
            };

            return paged;
        }

    }
}
