using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Controllers.Exceptions
{
    public class CountyNotFoundException : ControllerException
    {
        public CountyNotFoundException() : base("County not found", StatusCodes.Status400BadRequest)
        {
        }
    }

}
