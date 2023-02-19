using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using CovidService.Repositories;
using CovidService.Services.County.Extensions;
using CovidService.Services.State;

namespace CovidService.Services.County
{
    public class StateService : IStateService
    {
        private IRepository _repository;
        public StateService(IRepository repository)
        {
            _repository = repository;
        }

        public StateSummary GetSummary(string stateName, DateTime startDate, DateTime endDate)
        {
            var state = _repository.GetState(stateName);
            if (state == null)
                return null;

            var aggregatedSummary = state.Counties.Values.Summary(startDate, endDate).Aggregate(
                (current, next) =>
                {
                    //Math.Min(current.Cases.Minimum.Count, next.Cases.Minimum.Count);
                    //current.Cases.Maximum;

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

            return new StateSummary()
            {
                State = stateName,
                Cases = aggregatedSummary.Cases
            };

        }

    }
}
