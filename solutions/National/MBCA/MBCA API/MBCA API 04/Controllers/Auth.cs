using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MBCA_API_04.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MBCA_API_04.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class Auth(MBCAContext db) : ControllerBase {

        public class loginBody {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        

        public class registerBody {
            public string Username { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("Login")]
        public IActionResult login([FromBody] loginBody body) {
            var user = db.Users.Where(X => X.Username == body.Username && X.Password == body.Password).FirstOrDefault();

            if (user == null) {
                return NotFound("Credentials not found");
            } else if (user.RoleId == 2) {
                return BadRequest("Only visitor can login to the app");
            }

            var token = generateToken(user.Username);

            return Ok(token);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> register([FromBody] registerBody body) {
            var user = new User {
                Email = body.Email,
                FullName = body.FullName,
                IsActivated = 1,
                PhoneNumber = body.PhoneNumber,
                Username = body.Username,
                Password = body.Password,
                RoleId = 1,
            };

            if (db.Users.Any(X => X.Username == body.Username) ) {
                return BadRequest("Username is already used");
            }

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return Ok();
        }

        private string generateToken(string username) {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, username),
            };

            var token = new JwtSecurityToken(
                audience: "http://localhost:5000",
                claims: claims,
                issuer: "http://localhost:5000",
                expires: DateTime.Now.AddYears(20),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456789012345678901234567890")),
                    algorithm: SecurityAlgorithms.HmacSha256
                    )
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
