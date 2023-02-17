namespace CovidService.Services
{
    public class CasesSummary
    {
        public double Average { get; set; }
        public DateAndCount Minimum { get; set; }
        public DateAndCount Maximum { get; set; }
    }
}
