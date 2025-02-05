using MedicalSystemApi.Models.DTOs.Doctor;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("AllDoctors")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var doctorList = await _doctorService.GetAllAsync();
                return Ok(doctorList);
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

                var doctorDto = await _doctorService.GetByIdAsync(id);
                return Ok(doctorDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddDoctor")]
        public async Task<IActionResult> Add([FromForm] CreateDoctorDto createDoctorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _doctorService.AddAsync(createDoctorDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateDoctorDto updateDoctorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _doctorService.UpdateAsync(id, updateDoctorDto);
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
                await _doctorService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }

        [HttpGet("GetDoctorsBySpecialty/{specialty}")]
        public async Task<IActionResult> GetDoctorsBySpecialty(string specialty)
        {
            try
            {
                var doctors = await _doctorService.GetDoctorsBySpecialty(specialty);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAvailableDoctorsToday")]
        public async Task<IActionResult> GetAvailableDoctors()
        {
            try
            {
                var doctors = await _doctorService.GetAvailableDoctorsToDay();
                return Ok(doctors);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving available doctors.");
            }
        }

        [HttpPut("{doctorId}/update-working-hours")]
        public async Task<IActionResult> UpdateDoctorWorkingHours(int doctorId, [FromBody] string newWorkingHours)
        {
            try
            {
                await _doctorService.UpdateDoctorWorkingHoursAsync(doctorId, newWorkingHours);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{doctorId}/assign-department/{departmentId}")]
        public async Task<IActionResult> AssignDoctorToDepartment(int doctorId, int departmentId)
        {
            try
            {
                await _doctorService.AssignDoctorToDepartmentAsync(doctorId, departmentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
