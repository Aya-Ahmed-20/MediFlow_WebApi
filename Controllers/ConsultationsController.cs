using Asp.Versioning;
using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using MediFlowApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFlowApi.Controllers
{
    [Authorize(Roles = "Doctor")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[Controller]")]
    public class ConsultationsController:ControllerBase
    {
        private readonly IConsultationsService _service;
          public ConsultationsController(IConsultationsService iService)
           {
            _service = iService;
           }
        [HttpPost]
        public async Task<IActionResult> CreateConsultation(CreateConsultationDto dto)
        {
          var consId= await _service.CreateConsultationAsync(dto);
            return CreatedAtAction(nameof(ReadConsultation),new {id=consId },consId);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadConsultation(int id )
        {
            var cons =await _service.ReadConsultationAsync(id);
            return Ok(cons);

        }

    }
}
