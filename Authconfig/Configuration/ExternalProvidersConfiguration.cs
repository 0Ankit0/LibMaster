using Authconfig.Classes;
using Authconfig.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;

namespace Authconfig.Configuration
{
    /// <summary>
    /// Configures external providers for authentication.
    /// </summary>
    /// <remarks>
    /// Put the required fields in the appsettings.json file.
    ///  "Authentication": {
    ///     "Google": {
    ///       "ClientId": "your-google-client-id",
    ///       "ClientSecret": "your-google-client-secret"
    ///     },
    ///     "Facebook": {
    ///     "AppId": "your-facebook-app-id",
    ///       "AppSecret": "your-facebook-app-secret"
    ///     },
    ///     "Microsoft": {
    ///     "ClientId": "your-microsoft-client-id",
    ///       "ClientSecret": "your-microsoft-client-secret"
    ///     },
    ///     "Twitter": {
    ///     "ApiKey": "your-twitter-api-key",
    ///       "ApiSecretKey": "your-twitter-api-secret-key"
    ///     }
    ///   }
    /// To use this configuration, add the following to the project's Startup file:
    /// </remarks>
    /// <example>
    /// <code>
    /// var providerConfig = new ExternalProvidersConfiguration(builder.Configuration);
    /// providerConfig.ConfigureServices(builder.Services);
    /// </code>
    /// </example>
    public class ExternalProvidersConfiguration
    {
        private readonly IConfiguration _configuration;

        public ExternalProvidersConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ExternalLoginSettings>(_configuration.GetSection("Authentication"));

            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie();

            var googleSettings = _configuration.GetSection("Authentication:Google").Get<GoogleSettings>();
            if (googleSettings != null)
            {
                authenticationBuilder.AddGoogle(options =>
                {
                    options.ClientId = googleSettings.ClientId;
                    options.ClientSecret = googleSettings.ClientSecret;
                });
            }

            var facebookSettings = _configuration.GetSection("Authentication:Facebook").Get<FacebookSettings>();
            if (facebookSettings != null)
            {
                authenticationBuilder.AddFacebook(options =>
                {
                    options.AppId = facebookSettings.AppId;
                    options.AppSecret = facebookSettings.AppSecret;
                });
            }

            var microsoftSettings = _configuration.GetSection("Authentication:Microsoft").Get<MicrosoftSettings>();
            if (microsoftSettings != null)
            {
                authenticationBuilder.AddMicrosoftAccount(options =>
                {
                    options.ClientId = microsoftSettings.ClientId;
                    options.ClientSecret = microsoftSettings.ClientSecret;
                });
            }

            var twitterSettings = _configuration.GetSection("Authentication:Twitter").Get<TwitterSettings>();
            if (twitterSettings != null)
            {
                authenticationBuilder.AddTwitter(options =>
                {
                    options.ConsumerKey = twitterSettings.ApiKey;
                    options.ConsumerSecret = twitterSettings.ApiSecretKey;
                });
            }

            services.AddHttpContextAccessor();
            services.AddScoped<ExternalLogin>();
        }
    }
}
