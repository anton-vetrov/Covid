using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;

namespace CovidService.Services.Github
{
    public class DailyTimerService : IDailyTimerService
    {
        long _lastTicks = 0;

        public DailyTimerService(long lastTicks = 0)
        { 
            _lastTicks = lastTicks;
        }

        public bool IsNewDay 
        {
            get { 
                var currentTicks = DateTime.Now.Ticks;
                var lastTicks = Interlocked.Exchange(ref _lastTicks, currentTicks);

                var dayDiff = (new DateTime(currentTicks) - new DateTime(lastTicks)).Days;

                return dayDiff > 0;
            }
        }
    }
}
