namespace CovidService.Services
{
    public class CasesSummary
    {
        public int Average { get; set; }
        public DateAndCount Minimum { get; set; }
        public DateAndCount Maximum { get; set; }
    }
}
