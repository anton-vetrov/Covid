using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CovidService.Services.County.Extensions
{
    public static class StatExtension
    {
        public static DateTime _blankDateTime = new DateTime(0001, 01, 01);

        // TODO This traverses the list of cases three times
        public static IEnumerable<CountySummary> Summary(this IEnumerable<CovidService.Models.County> counties, DateTime startDate, DateTime endDate)
        {
            var summaries = new List<CountySummary>();
            foreach (var county in counties)
            {
                var cases = (startDate == _blankDateTime && endDate == _blankDateTime) 
                    ? county.Cases
                    : county.Cases.Where(x => x.Key >= startDate && x.Key <= endDate);

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

            return summaries;
        }

        public static IEnumerable<CountyBreakdown> BreakdownAndRate(this IEnumerable<CovidService.Models.County> counties, DateTime startDate, DateTime endDate)
        {
            var countyBreakdowns = new List<CountyBreakdown>();
            foreach (var county in counties)
            {
                var cases = (startDate == StatExtension._blankDateTime && endDate == StatExtension._blankDateTime)
                    ? county.Cases
                    : county.Cases.Where(x => x.Key >= startDate && x.Key <= endDate);
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
                        RatePercentage = (x.Value.Count == 0) ? 0 : ((double)(y.Value.Count - x.Value.Count)) * 100.00 / ((double)x.Value.Count),
                        Date = y.Key
                    }
                );

                countyBreakdowns.Add(new CountyBreakdown()
                {
                    County = county.CombinedKey,
                    DateBreakdowns = breakdown
                });
            }

            return countyBreakdowns;
        }

    }
}
