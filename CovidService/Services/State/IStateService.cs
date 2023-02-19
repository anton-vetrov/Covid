using System;
using System.Collections.Generic;

namespace CovidService.Services.State
{
    public interface IStateService
    {
        StateSummary GetSummary(string stateName, DateTime startDate, DateTime endDate);
        public StateBreakdown GetBreakdownAndRate(string stateName, DateTime startDate, DateTime endDate);
        public StateRate GetRate(string stateName, DateTime startDate, DateTime endDate);
    }
}
