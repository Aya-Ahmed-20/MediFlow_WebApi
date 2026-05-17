using MediFlowApi.DTOs;
using MediFlowApi.Models;
using Microsoft.AspNetCore.Identity;

namespace MediFlowApi.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
        Task<AuthResult> LoginAsync(LoginDto loginDto);

        
    }
}
