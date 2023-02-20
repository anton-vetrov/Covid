using System;

namespace CovidService
{
    public class BaseException : Exception
    {
        public BaseException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
    }
}
