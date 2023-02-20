using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Controllers.Exceptions
{
    public class BlankCountyException : BaseException
    {
        public BlankCountyException() : base("The county is blank. Please provide county.", StatusCodes.Status400BadRequest)
        {
        }
    }
}
