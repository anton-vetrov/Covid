using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Controllers.Exceptions
{
    public class BlankCountyException : Exception
    {
        public int StatusCode { get; set; }
        public BlankCountyException() : base($"The location is blank. Please provide county.")
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
