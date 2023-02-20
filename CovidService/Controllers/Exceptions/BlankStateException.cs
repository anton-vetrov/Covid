using Microsoft.AspNetCore.Http;

namespace CovidService.Controllers.Exceptions
{
    public class BlankStateException : BaseException
    {
        public BlankStateException() : base("The state is blank. Please provide state.", StatusCodes.Status400BadRequest)
        {
        }
    }
}
