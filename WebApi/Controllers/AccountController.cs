using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
    [Route("api/abp/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtBearer _options;

        public AccountController(IOptions<JwtBearer> options)
        {
            _options = options.Value;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<LoginOutputViewModel> Post()
        {
            return new LoginOutputViewModel()
            {
                Token = CreateAccessToken()
            };
        }

        private string CreateAccessToken()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, "User"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, "user@example.com"));
            
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKey));
            var siginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: siginCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
    
    public class LoginOutputViewModel
    {
        public string Token { get; set; }
    }

    /// <summary>
    /// Bearer options.
    /// </summary>
    public class JwtBearer
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
    }
}
