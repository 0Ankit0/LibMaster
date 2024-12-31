using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authconfig.Models
{
    /// <summary>
    /// Represents the settings for external login providers.
    /// </summary>
    public class ExternalLoginSettings
    {
        /// <summary>
        /// Gets or sets the Google login settings.
        /// </summary>
        public GoogleSettings? Google { get; set; }

        /// <summary>
        /// Gets or sets the Facebook login settings.
        /// </summary>
        public FacebookSettings? Facebook { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft login settings.
        /// </summary>
        public MicrosoftSettings? Microsoft { get; set; }

        /// <summary>
        /// Gets or sets the Twitter login settings.
        /// </summary>
        public TwitterSettings? Twitter { get; set; }
    }

    /// <summary>
    /// Represents the settings for Google login.
    /// </summary>
    public class GoogleSettings
    {
        /// <summary>
        /// Gets or sets the Google client ID.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Google client secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }

    /// <summary>
    /// Represents the settings for Facebook login.
    /// </summary>
    public class FacebookSettings
    {
        /// <summary>
        /// Gets or sets the Facebook app ID.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the Facebook app secret.
        /// </summary>
        public string AppSecret { get; set; }
    }

    /// <summary>
    /// Represents the settings for Microsoft login.
    /// </summary>
    public class MicrosoftSettings
    {
        /// <summary>
        /// Gets or sets the Microsoft client ID.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft client secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }

    /// <summary>
    /// Represents the settings for Twitter login.
    /// </summary>
    public class TwitterSettings
    {
        /// <summary>
        /// Gets or sets the Twitter API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the Twitter API secret key.
        /// </summary>
        public string ApiSecretKey { get; set; }
    }
}