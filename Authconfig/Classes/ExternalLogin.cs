using Authconfig.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Authconfig.Classes
{
    /// <summary>
    /// This class is used to configure external login providers.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="externalLoginSettings">The external login settings from appsettings.</param>
    public class ExternalLogin
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ExternalLoginSettings _externalLoginSettings;

        public ExternalLogin(IHttpContextAccessor httpContextAccessor, IOptions<ExternalLoginSettings> externalLoginSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _externalLoginSettings = externalLoginSettings.Value;
        }
        /// <summary>
        /// Initiates a challenge for Google authentication.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when Google settings are not configured.</exception>

        public async Task ChallengeGoogleAsync()
        {
            if (_externalLoginSettings.Google != null)
            {
                var properties = new AuthenticationProperties { RedirectUri = "/signin-google" };
                await _httpContextAccessor.HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties);
            }
            else
            {
                throw new InvalidOperationException("Google settings are not configured.");
            }
        }
        /// <summary>
        /// Initiates a challenge for Facebook authentication.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when Facebook settings are not configured.</exception>
        public async Task ChallengeFacebookAsync()
        {
            if (_externalLoginSettings.Facebook != null)
            {
                var properties = new AuthenticationProperties { RedirectUri = "/signin-facebook" };
                await _httpContextAccessor.HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, properties);
            }
            else
            {
                throw new InvalidOperationException("Facebook settings are not configured.");
            }
        }
        /// <summary>
        /// Initiates a challenge for Microsoft authentication.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when Microsoft settings are not configured.</exception>

        public async Task ChallengeMicrosoftAsync()
        {
            if (_externalLoginSettings.Microsoft != null)
            {
                var properties = new AuthenticationProperties { RedirectUri = "/signin-microsoft" };
                await _httpContextAccessor.HttpContext.ChallengeAsync(MicrosoftAccountDefaults.AuthenticationScheme, properties);
            }
            else
            {
                throw new InvalidOperationException("Microsoft settings are not configured.");
            }
        }
        /// <summary>
        /// Initiates a challenge for Twitter authentication.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when Twitter settings are not configured.</exception>

        public async Task ChallengeTwitterAsync()
        {
            if (_externalLoginSettings.Twitter != null)
            {
                var properties = new AuthenticationProperties { RedirectUri = "/signin-twitter" };
                await _httpContextAccessor.HttpContext.ChallengeAsync(TwitterDefaults.AuthenticationScheme, properties);
            }
            else
            {
                throw new InvalidOperationException("Twitter settings are not configured.");
            }
        }
        /// <summary>
        /// Signs out the current user.
        /// </summary>
        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
