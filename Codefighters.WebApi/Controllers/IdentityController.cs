using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeFighters.Data;
using CodeFighters.WebApi.Dto;
using CodeFighters.WebApi.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeFighters.Models;

namespace CodeFighters.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly ApiContext _context;
        private readonly IConfiguration _configuration;

        public IdentityController(ApiContext apiContext, IConfiguration configuration)
        {
            _context = apiContext;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login([FromBody]LoginDto loginDto)
        {
            var passwordHash = Encryption.PBKDF2(loginDto.Username, loginDto.Password);
            var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username && u.PasswordHash == passwordHash);
            if (user == null)
                return Unauthorized();

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                claims: new Claim[] { new Claim(ClaimTypes.Name, user.Username) }
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public IActionResult Register([FromBody]RegisterationDto registerDto)
        {
            var passwordHash = Encryption.PBKDF2(registerDto.Username, registerDto.Password);
            var user = new UserModel
            {
                Username = registerDto.Username,
                PasswordHash = passwordHash,
                DisplayName = registerDto.DisplayName,
                ProfilePictureUrl = "",
                Email = registerDto.Email
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }
    }
}
