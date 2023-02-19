using Microsoft.AspNetCore.Http;

namespace CovidService.Controllers.Exceptions
{
    public class BlankStateException : ControllerException
    {
        public BlankStateException() : base("The state is blank. Please provide state.", StatusCodes.Status400BadRequest)
        {
        }
    }
}
