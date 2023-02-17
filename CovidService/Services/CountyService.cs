using CovidService.Models;
using CovidService.Repositories;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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
        // TODO This traverses the list of cases three times
        public PagedCountySummary GetSummary(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            var summaries = new List<CountySummary>();

            IEnumerable<County> counties = _repository.GetCounties();

            if (!String.IsNullOrEmpty(countyName))
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
        public PagedCountyBreakdown GetBreakdown(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            var countyBreakdowns = new List<CountyBreakdown>();

            IEnumerable<County> counties = _repository.GetCounties();

            if (!String.IsNullOrEmpty(countyName))
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
                        NewCases = (y.Value.Count - x.Value.Count),
                        TotalCases = y.Value.Count,
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
        public PagedRate GetRate(string countyName, DateTime startDate, DateTime endDate, int pageIndex, int pageSize)
        {
            throw (new NotImplementedException());
        }

    }
}
