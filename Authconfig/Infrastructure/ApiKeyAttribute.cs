using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

// This attribute will be used to decorate controllers or actions that require an API key
// To use this attribute, simply add [ApiKey] to the controller or action
//In program.cs: builder.Services.AddScoped<ApiKeyFilter>();
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)] 
public class ApiKeyAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService(typeof(ApiKeyFilter)) as IFilterMetadata;
    }
}
