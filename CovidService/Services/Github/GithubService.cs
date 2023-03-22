using CovidService.Repositories;
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
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private Task<Stream> _downloadStreamTask;

        public GithubService(ILogger<GithubService> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        private static readonly object _lock = new Object();
        public Task<Stream> DownloadFile()
        {
            lock (_lock)
            {
                if (_downloadStreamTask == null)
                {
                    _downloadStreamTask = _httpClient
                        .GetStreamAsync(_configuration["CovidCasesUrl"])
                        .ContinueWith<Stream>((task) =>
                        {
                            if (task.Status == TaskStatus.RanToCompletion)
                                return task.Result;

                            throw new GithubException();
                        });
                    
                }
                return _downloadStreamTask;
            }
        }
    }
}
