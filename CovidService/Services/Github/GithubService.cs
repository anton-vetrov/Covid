using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidService.Services.Github
{
    public class GithubService : IGithubService
    {
        private readonly ILogger<GithubService> _logger;

        private Task<Stream> _downloadStreamTask;

        public GithubService(ILogger<GithubService> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _downloadStreamTask = httpClient.GetStreamAsync(configuration["CovidCasesUrl"]);
        }

        public async Task<Stream> DownloadFile()
        {
            if (_downloadStreamTask.Status == TaskStatus.RanToCompletion)
            {
                // TODO _logger.LogWarning("File download has finished!"); doesn't hit a mock 
                _logger.Log(
                    LogLevel.Warning,
                    0,
                    this,
                    null,
                    (state, exception) =>
                    {
                        return "File download has finished!";
                    }
                );
            }

            try
            {
                var stream = await _downloadStreamTask;
                return stream;
            }
            catch (Exception)
            {
                throw new GithubException();
            }
        }
    }
}
