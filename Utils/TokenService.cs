using ECommerce.Interfaces.IUtils;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Utils
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(IConfiguration configuration, ILogger<TokenService> logger,UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<string> GenerateJWTTokenAsync(ApplicationUser applicationUser)
        {
            var roles=await _userManager.GetRolesAsync(applicationUser);
            int expiryTime = _configuration.GetValue<int>("Jwt:AuthTokenExpiryTime");
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,applicationUser.Id),
                new Claim(ClaimTypes.Role,roles.FirstOrDefault()??"Customer")
            };
            var Token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(expiryTime),
                signingCredentials:credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public async Task<string?> GenerateMailVerificationTokenAsync(ApplicationUser applicationUser)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            return  System.Web.HttpUtility.UrlEncode(token); 
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
