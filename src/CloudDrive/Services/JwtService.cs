using CloudDrive.Interfaces;
using CloudDrive.Models;
using CloudDrive.Models.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace CloudDrive.Services
{
    public class JwtService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration config)
        {
            _configuration = config;
        }

        public AuthToken BuildToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.Now.AddMinutes(30);
            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            var result = new JwtSecurityTokenHandler().WriteToken(token);
            var authToken = new AuthToken(result, user.Login, ((DateTimeOffset)expiresAt).ToUnixTimeSeconds());
            return authToken;
        }
    }
}