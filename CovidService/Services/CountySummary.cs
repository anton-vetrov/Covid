namespace CovidService.Services
{
    public class CountySummary
    {
        public string County { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public CasesSummary Cases { get; set; }
    }
}
