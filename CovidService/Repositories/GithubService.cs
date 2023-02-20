using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidService.Repositories
{
    public class GithubService : IGithubService
    {
        private string _uri = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv";

        private readonly ILogger<GithubService> _logger;

        private Task<Stream> _downloadStreamTask;

        public GithubService(ILogger<GithubService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _downloadStreamTask = httpClient.GetStreamAsync(_uri);
        }

        public async Task<Stream> DownloadFile()
        {
            if (_downloadStreamTask.Status == TaskStatus.RanToCompletion)
            {
                // TODO _logger.LogWarning("File already downloaded!"); doesn't hit a mock 
                _logger.Log(
                    LogLevel.Warning,
                    0,
                    this,
                    null,
                    (state, exception) => {
                        return "File already downloaded!";
                    }
                );
            }

            return await _downloadStreamTask;
        }
    }
}
