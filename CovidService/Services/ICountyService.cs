﻿using System;
using System.Collections.Generic;

namespace CovidService.Services
{
    public interface ICountyService
    {
        public PagedCountySummary GetSummary(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
        public PagedCountyBreakdown GetBreakdown(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
        public PagedRate GetRate(string county, DateTime startDate, DateTime endDate, int pageIndex, int pageSize);
    }
}
