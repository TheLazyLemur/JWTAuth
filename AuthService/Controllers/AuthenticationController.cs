using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthService.Model;
using AuthService.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            var userEmailExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null || userEmailExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new {Status = "Error", Message = "User Already Exists"});
            }

            var x = await _userManager.FindByEmailAsync("");

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created,
                    new {Status = "Success", Message = "User Created Successfully"});
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new {Status = "Error", Message = "User Creation Failed"});
        }


        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            string basicAuthHeader = HttpContext.Request.Headers["Authorization"];
            var encodedString = basicAuthHeader.Split(" ")[1];
            var decodedString = UtilityFunctions.DecodeBase64String(encodedString);

            var passwordAndUserName = decodedString.Split(":");
            var username = passwordAndUserName[0];
            var password = passwordAndUserName[1];

            var user = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new {Status = "Failed", Message = "User Does Not Exist"});

            var userRole = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Role, "Admin")
            };

            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var issuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:IssuerSigningKey"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(+3),
                SigningCredentials =
                    new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            var retToken = new JwtSecurityTokenHandler().WriteToken(token);

            return StatusCode(StatusCodes.Status200OK, new {token = retToken});
        }

        [Route("Verifying")]
        [Authorize (Roles = "Admin")]
        [HttpGet]
        public IActionResult VerifyToken()
        {
            return Ok();
        }
    }
}