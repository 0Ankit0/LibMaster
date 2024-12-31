using Authconfig.Classes;
using Authconfig.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_TEMPLATE.Configuration
{
    /// <summary>
    /// Configures JWT (JSON Web Token) authentication.
    /// </summary>
    /// <remarks>
    /// To use this configuration, add the following to the project's Startup file:
    /// </remarks>
    /// <example>
    /// <code>
    /// // Instantiate the JWT configuration class
    /// var jwtConfig = new JwtConfiguration(builder.Configuration);
    ///  // Configure services using the instance
    ///  jwtConfig.ConfigureServices(builder.Services); 
    /// </code>
    /// </example> 
    public class JwtConfiguration
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance to be used for JWT settings.</param>
        public JwtConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure JWT settings
            services.Configure<JwtSettings>(_configuration.GetSection("JwtSettings"));
            //In the appsettings
            //"JwtSettings": {
            //    "SecretKey": "aBcDeFgHiJkLmNoPqRsTuVwXyZ0123456789!@#$%^&*()",
            //    "Issuer": "issuer",
            //    "Audience": "audience"
            //  },

            // Inject JwtSettings using IOptions
            services.AddSingleton<IJwtAuth>(serviceProvider =>
            {
                var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>();
                return new JwtAuth(jwtSettings);
            });

            // Configure JWT authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;
                    var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // Validates token expiry
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                            var usernameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);

                            if (userIdClaim != null && usernameClaim != null)
                            {
                                var user = new AuthenticatedUser
                                {
                                    UserId = userIdClaim.Value,
                                    Username = usernameClaim.Value
                                };

                                // Add the user object to the HttpContext items
                                context.HttpContext.Items["User"] = user;
                            }

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                var result = JsonSerializer.Serialize(new { message = "Token has expired" });
                                return context.Response.WriteAsync(result);
                            }
                            else
                            {
                                context.NoResult();
                                context.Response.StatusCode = 500;
                                context.Response.ContentType = "text/plain";
                                return context.Response.WriteAsync(context.Exception.ToString());
                            }
                        }
                    };
                });
        }
    }
}