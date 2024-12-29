using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ApiKeyFilter : IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    //private readonly MyDbContext _dbContext;

    //public ApiKeyFilter(MyDbContext dbContext)
    //{
    //    _dbContext = dbContext;
    //}

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
        //context.Result = new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
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
