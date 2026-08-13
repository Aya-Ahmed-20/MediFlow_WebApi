using MediFlowApi.DTOs;
using MediFlowApi.Models;
using MediFlowApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlowApi.Controllers
{
    [Authorize(Roles = "Doctor")]
    [ApiController]
    [Route("api/[Controller]")]
    public class PrescriptionsController:ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePrescription(CreatePrescriptionDto dto)
        {
          var prescription= await  _prescriptionService.CreatePrescriptionAsync(dto);
            return CreatedAtAction(nameof(GetPrescription), new {id=prescription.Id},prescription);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrescription(int id)
        {
           var prescription= await _prescriptionService.GetPrescriptionByIdAsync(id);
            return Ok(prescription);
        }
    }
}
