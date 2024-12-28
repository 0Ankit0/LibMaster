using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authconfig.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AuthConfig.Configuration
{
    public class JwtConfiguration
    {
        public static void ConfigureJwtAuthentication(JwtBearerOptions options, JwtSettings jwtSettings)
        {
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
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
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
        }
    }
}
