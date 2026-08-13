using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using MediFlowApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediFlowApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; // 👈 أضفنا RoleManager للتأكد من وجود الـ Role
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var res = await _userManager.CreateAsync(user, registerDto.Password);

            if (res.Succeeded)
            {
                // 1. تحديد الـ Role المطلوب (من الـ DTO أو Default كـ Doctor للتجربة)
                var roleName = string.IsNullOrWhiteSpace(registerDto.Role) ? "Doctor" : registerDto.Role;

                // 2. التأكد من أن الـ Role موجود في الداتا بيز، وإن لم يوجد ننشئه
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // 3. 👈 إسناد الـ Role للمستخدم الجديد (وهذا هو السطر الذي كان ناقصاً)
                await _userManager.AddToRoleAsync(user, roleName);
            }

            return res;
        }

        public async Task<AuthResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new AuthResult
                {
                    IsAuthenticated = false,
                    Message = "Wrong Email Or Password"
                };
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return new AuthResult
                {
                    IsAuthenticated = false,
                    Message = "Wrong Email Or Password"
                };
            }
            //roles
            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return new AuthResult
            {
                IsAuthenticated = true,
                Message = "Logged in Successfully!",
                UserName = user.UserName,
                Token = token
            };
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Double.Parse(_configuration["JWT:DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}