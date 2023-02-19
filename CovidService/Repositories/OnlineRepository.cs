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

namespace CovidService.Repositories
{
    public class OnlineRepository : IRepository
    {
        private readonly ILogger<OnlineRepository> _logger;

        IRepository _repository;

        public OnlineRepository(ILogger<OnlineRepository> logger)
        {
            _logger = logger;

            _logger.LogInformation("logger={0}", _logger);
            using (var memoryStream = new MemoryStream(Properties.Resources.Covid19ConfirmedUS))
            {
                _repository = new FileRepository(memoryStream);
            }
        }

        public State GetState(string stateName)
        {
            return _repository.GetState(stateName);
        }

        public List<County> GetCounties()
        {
            return _repository.GetCounties();
        }
    }

}
