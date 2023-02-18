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
            if (context.Exception is UnexpectedInputException inputException)
            {
                context.Result = new ObjectResult(
                    new {
                        Message = inputException.Message,
                    }
                )
                {
                    StatusCode = inputException.StatusCode
                };

                context.ExceptionHandled = true;
            }

            if (context.Exception is BlankCountException blankLocationException)
            {
                context.Result = new ObjectResult(
                    new
                    {
                        Message = blankLocationException.Message,
                    }
                )
                {
                    StatusCode = blankLocationException.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
