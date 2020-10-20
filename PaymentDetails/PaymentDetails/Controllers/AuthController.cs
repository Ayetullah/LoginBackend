using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentDetails.Models;

namespace PaymentDetails.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly PdDbContex _context;

        public AuthController(PdDbContex dbContex)
        {
            _context = dbContex;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserDto userDto)
        {
            if(userDto == null)
            {
                return BadRequest("Invalid client request");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userDto.UserName && x.Password == userDto.Password);
            if(user != null)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Administrator")
                };
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7500",
                    audience: "https://localhost:7500",
                    claims: claims,
                    expires:DateTime.Now.AddMinutes(1),
                    signingCredentials: signingCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }
    }
}
