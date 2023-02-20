using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Controllers.Exceptions
{
    public class UnexpectedDateRangeException : BaseException
    {
        public UnexpectedDateRangeException() : base("Incorrect date range", StatusCodes.Status400BadRequest)
        {
        }
    }
}
