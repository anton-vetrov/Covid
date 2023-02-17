using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Controllers.Exceptions
{
    public class UnexpectedInputException : Exception
    {
        public int StatusCode { get; set; }
        public UnexpectedInputException() : base($"Unexpected input parameters")
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
