using Baithuchanh2.Data;
using Baithuchanh2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Baithuchanh2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;

        public AuthController(IConfiguration configuration, DataContext dataContext)
        {
            _configuration = configuration;
            _dataContext = dataContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Kiểm tra nếu username tồn tại
            var user = await _dataContext.User
                .Where(c => c.UserName == request.UserName)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Kiểm tra mật khẩu
            if (user.Password != request.Password)
            {
                return Unauthorized(new { Message = "Invalid password" });
            }


            // Sinh token
            var token = GenerateJwtToken(user.UserName);

            // (Tuỳ chọn) Lưu token vào cơ sở dữ liệu, nếu cần:
            user.Token = token;
            await _dataContext.SaveChangesAsync();

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string userName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
               issuer: "localhost",
               audience: "localhost",
               claims: claims,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}