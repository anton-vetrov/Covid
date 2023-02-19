using Microsoft.AspNetCore.Http;

namespace CovidService.Controllers.Exceptions
{
    public class StateNotFoundException : ControllerException
    {
        public StateNotFoundException() : base("State not found", StatusCodes.Status400BadRequest)
        {
        }
    }
}
