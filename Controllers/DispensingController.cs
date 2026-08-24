using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using MediFlowApi.Models;
using MediFlowApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediFlowApi.Controllers
{

    [Authorize(Roles = "Pharmacist")]
    [ApiController]
    [Route("api/[Controller]")]
    public class DispensingController : ControllerBase
    {
        private readonly IDispensingService _dispensingService;
        public DispensingController(IDispensingService dispensingService)
        {
            _dispensingService = dispensingService;
        }
        [HttpPost]
        public async Task<IActionResult> Dispensingasync(DispensePrescriptionDto dto)
        {
            string pharmacistId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            await _dispensingService.DispensingAsync(dto, pharmacistId);

           return Ok(new { message = "Prescription dispensed successfully!" });
        }

    }
}
