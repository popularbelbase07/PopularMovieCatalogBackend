using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PopularMovieCatalogBackend.Filters
{
    public class ParseBadRequest : IActionFilter
    {
        /*
       ´* Transform the error into array: 
        If you mistakenly erase the frontend validation and you still have backend validation then
        the user does not know about the error.To understand the error by user we should make the proper method.

         */
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as IStatusCodeActionResult;
            if (result == null)
            {
                return;

            }
            var statusCode = result.StatusCode;
            if (statusCode == 400)
            {
                var response = new List<string>();
                var badRequestObjectResult = context.Result as BadRequestObjectResult;
                if (badRequestObjectResult.Value is string)
                {
                    response.Add(badRequestObjectResult.Value.ToString());
                }

                else if (badRequestObjectResult.Value is IEnumerable<IdentityError> errors)
                {
                    foreach(var error in errors)
                    {
                        response.Add(error.Description);
                    }
                }

                else
                {
                    foreach(var key in context.ModelState.Keys)
                    {
                        foreach(var error in context.ModelState[key].Errors)
                        {
                            response.Add($"{key}: {error.ErrorMessage}");   
                        }
                    }
                }
                context.Result = new BadRequestObjectResult(response);

            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            return;
        }
    }
}
