using Azure;
using Microsoft.AspNetCore.Mvc;

namespace PopularMovieCatalogBackend.APIBehavior
{
    public class BadRequestBehavior
    {
        // Api bad request by client

        /*
       ´* Transform the error into array: 
        If you mistakenly erase the frontend validation and you still have backend validation then
        the user does not know about the error.To understand the error by user we should make the proper method.

         */

        public static void Parse (ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = ActionContext =>
            {
                var response = new List<string>();
                foreach (var key in ActionContext.ModelState.Keys)
                {
                    foreach (var error in ActionContext.ModelState[key].Errors)
                    {
                        response.Add($"{key}: {error.ErrorMessage}");
                    }
                }
                return new BadRequestObjectResult(response);
            };
        }
    }
}
