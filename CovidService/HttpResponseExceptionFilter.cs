using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CovidService.Controllers.Exceptions;
using System.Net;

namespace CovidService
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        // Make it the last in the chain
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ControllerException controllerException)
            {
                context.Result = new ObjectResult(
                    new {
                        Message = controllerException.Message,
                    }
                )
                {
                    StatusCode = controllerException.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
