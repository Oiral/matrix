using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Matrix.Core.Exceptions;

public class ExceptionResponseFilter : IActionFilter, IOrderedFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not BaseMatrixException baseMatrixException)
        {
            return;
        }

        switch (baseMatrixException)
        {
            case BikeNotFoundException notFoundException:
                context.Result = new NotFoundObjectResult(notFoundException.Message);
                context.ExceptionHandled = true;
                break;
        }
    }

    //Put this exception at the end as this should be our final check
    public int Order => int.MaxValue - 100;
}