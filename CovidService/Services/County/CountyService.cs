using System;
using System.Collections.Generic;
using System.Linq;

using CovidService.Repositories;

namespace CovidService.Services.County
{
    public class CountyService : ICountyService
    {
        private IRepository _repository;
        public CountyService(IRepository repository)
        {
            _repository = repository;
        }
        // TODO This traverses the list of cases three times
        public PagedCountySummary GetSummary(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            var summaries = new List<CountySummary>();

            IEnumerable<CovidService.Models.County> counties = _repository.GetCounties();

            if (!string.IsNullOrEmpty(countyName))
                counties = counties.Where(x => x.Name == countyName);

            var paged = new PagedCountySummary()
            {
                CountySummaries = summaries,
                TotalPagesCount = counties.Count()
            };

            counties = counties.Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var county in counties)
            {
                var cases = county.Cases.Where(x => x.Key >= startDate && x.Key <= endDate);

                if (cases.Count() == 0)
                {
                    break;
                }

                // TODO This is super ineffective, but short and passes array multiple times finding max and then date
                var max = cases.Select(x => x.Value.Count).Max();
                var min = cases.Select(x => x.Value.Count).Min();

                summaries.Add(
                    new CountySummary()
                    {
                        County = county.CombinedKey,
                        Lat = county.Lat,
                        Long = county.Long,
                        Cases = new CasesSummary()
                        {
                            Average = Math.Round(cases.Select(x => x.Value.Count).Average(), 1),
                            Minimum = new DateAndCount()
                            {
                                Count = min,
                                Date = cases.Where(x => x.Value.Count == min).First().Value.Date
                            },
                            Maximum = new DateAndCount()
                            {
                                Count = max,
                                Date = cases.Where(x => x.Value.Count == max).First().Value.Date
                            }
                        }
                    }
                );
            }
            //}

            return paged;
        }
        public PagedCountyBreakdown GetBreakdownAndRate(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            var countyBreakdowns = new List<CountyBreakdown>();

            IEnumerable<CovidService.Models.County> counties = _repository.GetCounties();

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
                var cases = county.Cases.Where(x => x.Key >= startDate && x.Key <= endDate);
                if (cases.Count() == 0)
                {
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

        public PagedCountyRate GetRate(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            var breakdown = GetBreakdownAndRate(countyName, startDate, endDate, pageIndex, pageSize);

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
