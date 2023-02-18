using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Controllers.Exceptions
{
    public class BlankCountException : Exception
    {
        public int StatusCode { get; set; }
        public BlankCountException() : base($"The location is blank. Please provide county.")
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
