using CovidService.Models;
using CovidService.Repositories;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CovidService.Services
{
    public class CountyService : ICountyService
    {
        private IRepository _repository;
        public CountyService(IRepository repository)
        {
            _repository = repository;
        }
        public PagedCountySummary GetSummary(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            var summaries = new List<CountySummary>();

            IEnumerable<County> counties = _repository.GetCounties();
            // TODO This traverses the list of cases three times
            if (!String.IsNullOrEmpty(countyName))
                counties = counties.Where(x => x.Name == countyName);

            var countySummary = new PagedCountySummary()
            {
                CountySummary = summaries,
                TotalPagesCount = counties.Count()
            };

            counties = counties.Skip(pageIndex * pageSize)
                .Take(pageSize);

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

            return countySummary;
        }
        public Breakdown GetBreakDown()
        {
            throw (new NotImplementedException());
        }
        public Rate GetRate()
        {
            throw (new NotImplementedException());
        }

    }
}
