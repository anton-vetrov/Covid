using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Services.Exceptions
{
    public class EmptyResultException : BaseException
    {
        // TODO Consider request culture for the date
        public EmptyResultException(DateTime lastDate) : base($"No records found. Last available COVID data is dated back to {lastDate:MM/dd/yyyy}", StatusCodes.Status404NotFound)
        { 
        }
    }
}
