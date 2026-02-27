using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API06.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace API06.Controllers {
    [Route("gsa-api/v1/users")]
    [ApiController]
    public class Users(GSAContext context) : ControllerBase {

        public class RegisterModel {
            [Required] public string username { get; set; }
            [Required] public string fullname { get; set; }
            [Required] public string email { get; set; }
            [Required] public string password { get; set; }
        }



        private string hashPassword(string password) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes) {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        [HttpPost("register")]
        [Consumes("application/json")]
        public async Task<IActionResult> Register([FromBody][Required] RegisterModel model) {

            try {
                var mail = new MailAddress(model.email);
            } catch {
                return UnprocessableEntity(new response { message = "Validation error: email is invalid." });
            }

            if (context.Users.Any(x => x.Email == model.email)) {
                return UnprocessableEntity(new response { message = "Email must be unique" });
            }

            if (
                model.password.Any(x => char.IsDigit(x)) &&
                model.password.Any(x => char.IsUpper(x)) &&
                model.password.Any(x => char.IsLower(x)) &&
                model.password.Any(x => char.IsSymbol(x)) &&
                model.password.Length > 7
                ) {

                var hashedPassword = hashPassword(model.password);

                var user = new Models.User {
                    Name = model.fullname,
                    Username = model.username,
                    Email = model.email,
                    PasswordHash = hashedPassword,
                    Role = "student",
                    CreatedAt = DateTime.Now,
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Ok(new response { message = "User registered successfully." });

            } else {
                return UnprocessableEntity(new response { message = "Password must be minimum 8 characters with an uppercase, a lowercase, a symbol, and a number" });
            }

        }

        public class LoginModel {
            [Required] public string Email { get; set; }
            [Required] public string Password { get; set; }
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> login([FromBody][Required] LoginModel loginModel) {
            var hashedPassword = hashPassword(loginModel.Password);

            try {
                var mail = new MailAddress(loginModel.Email);
            } catch {
                return UnprocessableEntity(new response { message = "Validation error: email is invalid." });
            }

            if (loginModel.Password.Length > 7 == false) {
                return UnprocessableEntity(new response { message = "Validation error: password must be at least 8 characters." });
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == loginModel.Email && x.PasswordHash == hashedPassword);

            if (user == null) {
                return Unauthorized(new response { message = "Invalid email or password." });
            }

            var token = GenerateToken(user.Email, user.Role);

            return Ok(new responseLogin {
                message = new response { message = "Login successful." },
                data = new Data {
                    userId = user.Id,
                    username = user.Username,
                    role = user.Role,
                    token = token
                }
            });
        }



        public class response {
            [Required] public string message { get; set; }
        }

        public class responseLogin {
            [Required] public response message { get; set; }
            [Required] public Data data { get; set; }
        }

        public class Data {
            [Required] public int userId { get; set; }
            [Required] public string username { get; set; }
            [Required] public string role { get; set; }
            [Required] public string token { get; set; }
        }

        private string GenerateToken(string email, string role) {
            var claim = new List<Claim> {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                audience: "http://localhost:5000",
                issuer: "http://localhost:5000",
                claims: claim,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456789012345678901234567890")),
                    algorithm: SecurityAlgorithms.HmacSha256
                )
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("logout")]
        public IActionResult Logout() {

            if (User.Claims.Count() == 0) {
                return Unauthorized(new response { message = "Authorization token missing or invalid." });
            }

            return Ok(new response { message = "Logout successful." });
        }
    }
}
