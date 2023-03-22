using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CovidService.Services.County.Extensions
{
    public static class CountyExtensions
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

                var minimum = cases.Aggregate((current, next) => {
                    var date = (current.Value.Count <= next.Value.Count) ? current.Value.Date : next.Value.Date;
                    return new KeyValuePair<DateTime, Models.Case>(
                        date,
                        new Models.Case()
                        {
                            Date = date,
                            Count = (current.Value.Count <= next.Value.Count) ? current.Value.Count : next.Value.Count
                        });
                    
                });
                var maximum = cases.Aggregate((current, next) => {
                    var date = (current.Value.Count >= next.Value.Count) ? current.Value.Date : next.Value.Date;
                    return new KeyValuePair<DateTime, Models.Case>(
                        date,
                        new Models.Case()
                        {
                            Date = date,
                            Count = (current.Value.Count >= next.Value.Count) ? current.Value.Count : next.Value.Count
                        });

                });
                var average = Math.Round(cases.Select(x => x.Value.Count).Average(), 1);

                summaries.Add(
                    new CountySummary()
                    {
                        County = county.CombinedKey,
                        Lat = county.Lat,
                        Long = county.Long,
                        Cases = new CasesSummary()
                        {
                            Average = average,
                            Minimum = new DateAndCount()
                            {
                                Count = minimum.Value.Count,
                                Date = minimum.Value.Date
                            },
                            Maximum = new DateAndCount()
                            {
                                Count = maximum.Value.Count,
                                Date = maximum.Value.Date
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
                var cases = (startDate == CountyExtensions._blankDateTime && endDate == CountyExtensions._blankDateTime)
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
