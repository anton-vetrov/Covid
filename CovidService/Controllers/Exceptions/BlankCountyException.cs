using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Controllers.Exceptions
{
    public class BlankCountyException : ControllerException
    {
        public BlankCountyException() : base($"The location is blank. Please provide county.", StatusCodes.Status400BadRequest)
        {
        }
    }
}
