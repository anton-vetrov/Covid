using System.Collections.Generic;

namespace CovidService.Services
{
    public class PagedRate
    {
        public IEnumerable<Rate> CountyRate { get; set; }
        public int TotalPagesCount { get; set; }
    }
}
