using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using MediFlowApi.Models;
using Microsoft.AspNetCore.Identity;

namespace MediFlowApi.Services
{
    public class AuthService : IAuthService
    {
      private readonly  UserManager<ApplicationUser> _userManager;
        public AuthService(UserManager<ApplicationUser> userManager)
        { 
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName=registerDto.Email
            };
            var res= await _userManager.CreateAsync(user,registerDto.Password);
            return res;
        }

        public async Task<AuthResult> LoginAsync(LoginDto loginDto) 
        { 
           var user=await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) 
            {
                return new AuthResult
                {
                    IsAuthenticated = false,
                    Message = "Wrong Email Or Password"
                };
            }
            bool isPasswordValid=await _userManager.CheckPasswordAsync(user,loginDto.Password);
            if (!isPasswordValid) 
            {
                return new AuthResult
                {
                    IsAuthenticated = false,
                    Message = "Wrong Email Or Password"
                };
            }
            return new AuthResult
            {
                IsAuthenticated = true,
                Message = "Logged in Successfully!",
                UserName=user.UserName
            };
        }
    }
}
