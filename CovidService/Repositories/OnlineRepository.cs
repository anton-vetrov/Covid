using CovidService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using System.Globalization;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CovidService.Services;
using CovidService.Controllers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CovidService.Repositories
{
    public class OnlineRepository : IRepository
    {
        private readonly ILogger<OnlineRepository> _logger;

        // TODO Eliminate this
        IRepository _syncRepository = new FileRepository(new MemoryStream(Properties.Resources.Covid19ConfirmedUS));

        IRepository _repository;
        IGithubService _githubService;

        public OnlineRepository(ILogger<OnlineRepository> logger, IGithubService githubService)
        {
            _logger = logger;
            _githubService = githubService;
        }

        private static readonly object _lock = new Object();
        /// <summary>
        /// This avoids multiple parsings of the file. File repository may need to downlaod once a day
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private IRepository GetFileRepository(Stream stream)
        {
            lock (_lock)
            {
                if (_repository == null)
                {
                    _repository = new FileRepository(stream);
                }

                return _repository;
            }
        }

        public async Task<State> GetStateAsync(string stateName)
        {
            var stream = await _githubService.DownloadFile();
            return await GetFileRepository(stream).GetStateAsync(stateName);
        }

        public async Task<List<County>> GetCountiesAsync()
        {
            var stream = await _githubService.DownloadFile();
            return await GetFileRepository(stream).GetCountiesAsync();
        }
    }

}
