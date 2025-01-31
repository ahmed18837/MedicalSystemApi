using MedicalSystemApi.Models.DTOs.Patient;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("AllPatients")]
        public async Task<IActionResult> AllStaff()
        {
            try
            {
                var staff = await _patientService.GetAllAsync();
                return Ok(staff);
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

                var patientDto = await _patientService.GetByIdAsync(id);
                return Ok(patientDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddPatient")]
        public async Task<IActionResult> Add(CreatePatientDto createPatientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _patientService.AddAsync(createPatientDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto updatePatientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _patientService.UpdateAsync(id, updatePatientDto);
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
                await _patientService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }
    }
}
