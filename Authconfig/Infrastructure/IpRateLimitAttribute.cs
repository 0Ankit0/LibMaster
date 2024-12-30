
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

/// <summary>
/// This attribute is used to limit the number of requests that can be made by a single IP address in a given time window.
/// </summary>
/// <remarks>
/// To use this attribute, add it to the action method in the controller like this:
/// <code>
/// [IpRateLimit(limit=20, time=100)]
/// </code>
/// </remarks>
/// <param name="limit">The maximum number of requests allowed from a single IP address within the specified time window.</param>
/// <param name="time">The time window in seconds during which the request count is tracked.</param>
/// <example>
/// <code>
/// [IpRateLimit(limit=20, time=100)]
/// public IActionResult MyAction()
/// {
///     // Action implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class IpRateLimitAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _limit;
    private readonly TimeSpan _timeWindow;

    public IpRateLimitAttribute(int limit, int time)
    {
        _limit = limit;
        _timeWindow = TimeSpan.FromSeconds(time);
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
        var cacheKey = $"IpRateLimit_{ipAddress}";
        var requestCount = cache.Get<int>(cacheKey);

        if (requestCount >= _limit)
        {
            context.Result = new ContentResult
            {
                StatusCode = 429, // Too Many Requests
                Content = "IP rate limit exceeded. Try again later."
            };
            return;
        }

        cache.Set(cacheKey, requestCount + 1, _timeWindow);

        await next();
    }
}
