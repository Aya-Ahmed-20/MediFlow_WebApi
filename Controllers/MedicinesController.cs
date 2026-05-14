using Asp.Versioning;
using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using MediFlowApi.Models;
using MediFlowApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace MediFlowApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[Controller]")]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineServices _medicineService;
        public MedicinesController(IMedicineServices medicineService)
        {
            _medicineService = medicineService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? term,
           [FromQuery] int pageSize, [FromQuery] int pageNumber)
        { 
            var med =await _medicineService.GetAllAsync(term, pageSize, 
                pageNumber);
            return Ok(med);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MedicineCreateDto medicineDto)
        {
            // لسنا بحاجة لفحص ModelState يدوياً إذا كان الكلاس عليه [ApiController]
            // لأن .NET تقوم بذلك تلقائياً وترجع 400 Bad Request

            var createdMedicine = await _medicineService.AddAsync(medicineDto);

            // استخدام GetById ليرجع للمستخدم رابط الوصول للعنصر الجديد
            return CreatedAtAction(nameof(GetById), new { id = createdMedicine.Id }, createdMedicine);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id) {
          var med= await _medicineService.GetByIdAsync(id);
            if (med == null)
                return NotFound();
           return Ok(med);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,[FromBody]MedicineReadDto dto)
        {
            var success = await _medicineService.UpdateAsync(id, dto);
          if (!success) 
            {
            return NotFound();

            }
          return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var isExisting = await _medicineService.GetByIdAsync(id);
            if (isExisting == null)
            {
                return BadRequest();
            }
            await _medicineService.DeleteAsync(id);
            return NoContent();
        }
    }
}
 