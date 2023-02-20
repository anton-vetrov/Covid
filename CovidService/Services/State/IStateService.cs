using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidService.Services.State
{
    public interface IStateService
    {
        Task<StateSummary> GetSummary(string stateName, DateTime startDate, DateTime endDate);
        Task<StateBreakdown> GetBreakdownAndRate(string stateName, DateTime startDate, DateTime endDate);
        Task<StateRate> GetRate(string stateName, DateTime startDate, DateTime endDate);
    }
}
