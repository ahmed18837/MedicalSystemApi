using MedicalSystemApi.Models.DTOs.Medication;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationController : ControllerBase
    {
        private readonly IMedicationService _medicationService;

        public MedicationController(IMedicationService medicationService)
        {
            _medicationService = medicationService;
        }

        [HttpGet("AllMedications")]
        public async Task<IActionResult> AllStaff()
        {
            try
            {
                var medicationList = await _medicationService.GetAllAsync();
                return Ok(medicationList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var medicationDto = await _medicationService.GetByIdAsync(id);
                return Ok(medicationDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddMedication")]
        public async Task<IActionResult> Add(CreateMedicationDto createMedicationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _medicationService.AddAsync(createMedicationDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicationDto updateMedicationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _medicationService.UpdateAsync(id, updateMedicationDto);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _medicationService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }
    }
}
