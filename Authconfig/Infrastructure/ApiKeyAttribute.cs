using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

// This attribute can be used to require an API key for a controller action
// To use this attribute, add it to the action method in the controller like this:
// [ApiKey]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Uncomment the following code to check the API key against a database
        //var dbContext = context.HttpContext.RequestServices.GetService(typeof(MyDbContext)) as MyDbContext;
        //if (dbContext == null)
        //{
        //    context.Result = new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
        //    return;
        //}

        //var apiKey = await dbContext.ApiKeys.Include(k => k.User).FirstOrDefaultAsync(k => k.Key == potentialApiKey);
        //if (apiKey == null)
        //{
        //    context.Result = new UnauthorizedResult();
        //    return;
        //}

        // Add user info to HttpContext.Items so it can be accessed in the controller
        //context.HttpContext.Items["User"] = apiKey.User;

        await next();
    }
}
