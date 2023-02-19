using System;
using System.Collections.Generic;

namespace CovidService.Services.State
{
    public interface IStateService
    {
        StateSummary GetSummary(string stateName, DateTime startDate, DateTime endDate);
        /*
        public PagedCountyBreakdown GetBreakdownAndRate(string state, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
        public PagedCountyRate GetRate(string state, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
        */
    }
}
