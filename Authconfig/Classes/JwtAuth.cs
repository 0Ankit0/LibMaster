using Authconfig.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace Authconfig.Classes
{
    /// <summary>
    /// Interface for JWT authentication.
    /// </summary>
    public interface IJwtAuth
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A JWT token as a string.</returns>
        string GenerateToken(string username, string userId);
    }

    /// <summary>
    /// Implementation of the IJwtAuth interface for JWT authentication.
    /// </summary>
    public class JwtAuth : IJwtAuth
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SymmetricSecurityKey _signingKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtAuth"/> class.
        /// </summary>
        /// <param name="jwtSettings">The JWT settings from appsettings.json.</param>
        public JwtAuth(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A JWT token as a string.</returns>
        public string GenerateToken(string username, string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Issued at
                new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Not before
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddHours(_jwtSettings.TokenLifetimeHours).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Expiration
                new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience ?? string.Empty), // Audience
                new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer ?? string.Empty) // Issuer
            }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.TokenLifetimeHours),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}