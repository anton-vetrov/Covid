using Microsoft.AspNetCore.Http;

namespace CovidService.Controllers.Exceptions
{
    public class StateNotFoundException : BaseException
    {
        public StateNotFoundException() : base("State not found", StatusCodes.Status400BadRequest)
        {
        }
    }
}
