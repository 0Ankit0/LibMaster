using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

/// <summary>
/// This attribute is used to limit the number of requests that can be made by a single API key in a given time window.
/// </summary>
/// <remarks>
/// To use this attribute, add it to the action method in the controller like this:
/// <code>
/// [ApiKeyRateLimit(limit=20, time=100)]
/// </code>
/// </remarks>
/// <example>
/// <code>
/// [ApiKeyRateLimit(limit=20, time=100)]
/// public async Task<IActionResult> MyAction()
/// {
///     // Action implementation
/// }
/// </code>
/// </example>
/// <param name="limit">The maximum number of requests allowed within the specified time window.</param>
/// <param name="time">The time window in seconds during which the request count is measured.</param>
/// <exception cref="UnauthorizedResult">Thrown when the API key is missing from the request headers.</exception>
/// <exception cref="ContentResult">Thrown when the API key rate limit is exceeded.</exception>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ApiKeyRateLimitAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly int _limit;
    private readonly TimeSpan _timeWindow;

    public ApiKeyRateLimitAttribute(int limit, int time)
    {
        _limit = limit;
        _timeWindow = TimeSpan.FromSeconds(time);
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
        var cacheKey = $"ApiKeyRateLimit_{apiKey}";
        var requestCount = cache.Get<int>(cacheKey);

        if (requestCount >= _limit)
        {
            var retryAfter = _timeWindow.TotalSeconds;
            context.Result = new ContentResult
            {
                StatusCode = 429, // Too Many Requests
                Content = $"API key rate limit exceeded. Try again after {retryAfter} seconds."
            };
            return;
        }

        cache.Set(cacheKey, requestCount + 1, _timeWindow);

        await next();
    }
}
