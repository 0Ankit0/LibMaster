using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authconfig.Models
{
    /// <summary>
    /// Represents the settings required for JWT (JSON Web Token) authentication.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Gets or sets the secret key used to sign the JWT.
        /// </summary>
        public string? SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the JWT.
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience for the JWT.
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of the JWT in hours.
        /// Default value is 10 hours.
        /// </summary>
        public int TokenLifetimeHours { get; set; } = 10;
    }
    public class AuthenticatedUser
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
    }
}
