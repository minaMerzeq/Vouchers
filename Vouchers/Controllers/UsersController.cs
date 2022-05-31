using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Dtos;

namespace Vouchers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserSignDto userSignDto)
        {
            var identityUser = new IdentityUser { UserName = userSignDto.Email, Email = userSignDto.Email };
            var result = await _userManager.CreateAsync(identityUser, userSignDto.Password);

            if (result.Succeeded)
            {
                if (userSignDto.Email == "admin@gmail.com")
                {
                    await CreateAdminRoleIfNotExists();
                    var user = await _userManager.FindByEmailAsync(userSignDto.Email);
                    await _userManager.AddToRoleAsync(user, "Admin");
                }

                return await Login(userSignDto);
            }
            else
            {
                return BadRequest(new { errors = result.Errors.First().Description });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserSignDto userSignDto)
        {
            var checkPasswordRes = await CheckPassword(userSignDto);
            if (checkPasswordRes.IsAuthenticated)
            {
                return GenerateTokenResponse(checkPasswordRes.UserId, userSignDto.Email, checkPasswordRes.UserRole);
            }

            return Unauthorized();
        }

        private async Task<CheckPasswordResult> CheckPassword(UserSignDto creds)
        {
            var user = await _userManager.FindByNameAsync(creds.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, creds.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return new CheckPasswordResult()
                    {
                        UserId = user.Id,
                        IsAuthenticated = true,
                        UserRole = roles.Count > 0 ? roles[0] : "User"
                    };
                }
            }
            return new CheckPasswordResult()
            {
                UserId = null,
                IsAuthenticated = false,
                UserRole = null
            };

        }

        class CheckPasswordResult
        {
            public string UserId { get; set; }
            public bool IsAuthenticated { get; set; }
            public string UserRole { get; set; }
        }

        private IActionResult GenerateTokenResponse(string userId, string email, string userRole)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler(); 
            byte[] secret = Encoding.ASCII.GetBytes(_config["jwtSecret"]);
            
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim( ClaimTypes.NameIdentifier, userId) ,
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, userRole),
                }),
                Expires = DateTime.UtcNow.AddDays(180),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = handler.CreateToken(descriptor);
            return Ok(new
            {
                success = true,
                token = handler.WriteToken(token),
            });
        }

        private async Task CreateAdminRoleIfNotExists()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole { Name = "Admin" };
                await _roleManager.CreateAsync(adminRole);
            }
        }
    }
}
