﻿using CovidService.Controllers.Exceptions;
using CovidService.Models;
using CovidService.Services.County;
using CovidService.Services.County.Extensions;
using CovidService.Services.State;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidService.Controllers
{
    [ApiController]
    [Route("CovidApi/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly ILogger<StateController> _logger;
        private Services.State.IStateService _stateService;

        public StateController(ILogger<StateController> logger, Services.State.IStateService stateService)
        {
            _logger = logger;
            _stateService = stateService;

            _logger.LogInformation("stateService={0}", stateService);
        }


        /// <summary>
        /// Throws CountyNotFoundException
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Summary")]
        public Services.State.StateSummary GetSummary(string state, DateTime startDate, DateTime endDate) 
        {
            ValidateInput(state, startDate, endDate);

            var summary = _stateService.GetSummary(state, startDate, endDate);
            if (summary == null)
                throw (new StateNotFoundException());

            return summary;
        }

        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() => Problem();

        /// <summary>
        /// throws BlankCountyException, UnexpectedInputException
        /// </summary>
        /// <param name="state"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void ValidateInput(string state, DateTime startDate, DateTime endDate)
        {
            if (String.IsNullOrEmpty(state))
            {
                _logger.LogError("The state is blank");
                throw (new BlankStateException());
            }

            if (endDate < startDate || startDate == StatExtension._blankDateTime)
            {
                _logger.LogError("Unexpected date range {}-{}", startDate, endDate);
                throw (new UnexpectedInputException());
            }


            //_logger.LogInformation("{}, {}, {}", county, startDate, endDate);
        }
    }
}
