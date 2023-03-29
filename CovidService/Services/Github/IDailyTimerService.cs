namespace CovidService.Services.Github
{
    public interface IDailyTimerService
    {
        bool IsNewDay { get; }
    }
}
