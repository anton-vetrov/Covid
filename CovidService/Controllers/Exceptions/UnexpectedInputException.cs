using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Controllers.Exceptions
{
    public class UnexpectedInputException : ControllerException
    {
        public UnexpectedInputException() : base("Unexpected input parameters", StatusCodes.Status400BadRequest)
        {
        }
    }
}
