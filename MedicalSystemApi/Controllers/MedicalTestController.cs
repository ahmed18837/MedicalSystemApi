using MedicalSystemApi.Models.DTOs.MedicalTest;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalTestController : ControllerBase
    {
        private readonly IMedicalTestService _medicalTestService;

        public MedicalTestController(IMedicalTestService medicalTestService)
        {
            _medicalTestService = medicalTestService;
        }

        [HttpGet("AllMedicalTests")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var medicationTestList = await _medicalTestService.GetAllAsync();
                return Ok(medicationTestList);
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

                var medicationTestDto = await _medicalTestService.GetByIdAsync(id);
                return Ok(medicationTestDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddMedicationTest")]
        public async Task<IActionResult> Add(CreateMedicalTestDto createMedicalTestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _medicalTestService.AddAsync(createMedicalTestDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalTestDto updateMedicalTestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _medicalTestService.UpdateAsync(id, updateMedicalTestDto);
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
                await _medicalTestService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }
    }
}
