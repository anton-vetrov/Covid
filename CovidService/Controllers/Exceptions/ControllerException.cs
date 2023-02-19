using System;

namespace CovidService.Controllers.Exceptions
{
    public class ControllerException : Exception
    {
        public ControllerException(string message, int statusCode) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
    }
}
