using Asp.Versioning;
using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MediFlowApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[Controller]")]
    public class AccountController: ControllerBase
    {
      
        private readonly IAuthService _authService;
       public AccountController(IAuthService authService)
        {
            _authService= authService ;
        }
     
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto dto)
        {
           var res= await _authService.RegisterAsync(dto);
            if (res.Succeeded) {
                return Ok(new {message= "User Registered Successfully!"});
            }
            var errors = res.Errors.Select(e => e.Description);
            return BadRequest(new {Errors=errors});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            var res=await _authService.LoginAsync(loginDto);
            if (!res.IsAuthenticated)
            {
                return Unauthorized(new { message=res.Message});
            }
            return Ok(res);
        }
    }
}
