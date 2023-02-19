using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using CovidService.Repositories;
using CovidService.Services.County.Extensions;
using CovidService.Services.State;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CovidService.Services.County
{
    public class StateService : State.IStateService
    {
        private IRepository _repository;
        public StateService(IRepository repository)
        {
            _repository = repository;
        }

        public State.StateSummary GetSummary(string stateName, DateTime startDate, DateTime endDate)
        {
            var state = _repository.GetState(stateName);
            if (state == null)
                return null;

            var aggregatedSummary = state.Counties.Values.Summary(startDate, endDate).Aggregate(
                (current, next) =>
                {
                    return new CountySummary()
                    {
                        Cases = new CasesSummary()
                        {
                            Average = current.Cases.Average + next.Cases.Average,
                            Minimum = (current.Cases.Minimum.Count < next.Cases.Minimum.Count)? current.Cases.Minimum: next.Cases.Minimum,
                            Maximum = (current.Cases.Maximum.Count > next.Cases.Maximum.Count) ? current.Cases.Maximum : next.Cases.Maximum
                        }
                    };
                }
            );

            aggregatedSummary.Cases.Average /= (double)state.Counties.Count();

            return new State.StateSummary()
            {
                State = stateName,
                Cases = aggregatedSummary.Cases
            };

        }

        public StateBreakdown GetBreakdownAndRate(string stateName, DateTime startDate, DateTime endDate)
        {
            var state = _repository.GetState(stateName);
            if (state == null)
                return null;

            var countiesCount = state.Counties.Count();

            var aggregatedBreakdownAndRate = state.Counties.Values.BreakdownAndRate(startDate, endDate).Aggregate(
                (current, next) =>
                {
                    return new CountyBreakdown()
                    {
                        DateBreakdowns = current.DateBreakdowns.Zip(
                            next.DateBreakdowns,
                            (current, next) => {
                                return new DateBreakdown() { 
                                    Date = next.Date,
                                    TotalCases = current.TotalCases + next.TotalCases,
                                    NewCases = current.NewCases + next.NewCases,
                                    RatePercentage = current.RatePercentage + next.RatePercentage
                                };
                            }
                        )
                    };
                }
            );

            var averagedBreakdownAndRate = aggregatedBreakdownAndRate.DateBreakdowns.Select(
                x => new DateBreakdown() {
                    Date = x.Date,
                    TotalCases = x.TotalCases,
                    NewCases = x.NewCases, 
                    RatePercentage = (x.TotalCases == 0)? 0: (double)x.NewCases / (double)x.TotalCases * 100.0
                }                
            );

            return new StateBreakdown() { 
                State = state.Name,
                DateBreakdowns = averagedBreakdownAndRate
            };
        }

        public StateRate GetRate(string state, DateTime startDate, DateTime endDate)
        {
            var breakdown = GetBreakdownAndRate(state, startDate, endDate);

            var rate = new StateRate() { 
                State = breakdown.State,
                DateRates = breakdown.DateBreakdowns.Select(x => new DateRate() {
                    Date = x.Date,
                    Percentage = x.RatePercentage
                })
            };

            return rate;
        }

    }
}
