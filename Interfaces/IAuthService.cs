using MediFlowApi.DTOs;
using Microsoft.AspNetCore.Identity;

namespace MediFlowApi.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
       
        
    }
}
