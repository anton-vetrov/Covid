namespace CovidService.Services
{
    public interface ICountyService
    {
        public CountySummary GetSummary();
        public Breakdown GetBreakDown();
        public Rate GetRate();
    }
}
