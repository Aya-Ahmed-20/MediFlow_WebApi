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
    }
}
