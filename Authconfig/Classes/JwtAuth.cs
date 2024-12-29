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
    public interface IJwtAuth
    {
        string GenerateToken(string username, string userId);
    }

    public class JwtAuth : IJwtAuth
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SymmetricSecurityKey _signingKey;

        public JwtAuth(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));
        }

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
                    new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience), // Audience
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer) // Issuer
                   
                    // Add more claims here as needed
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