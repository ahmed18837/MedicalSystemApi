using MedicalSystemApi.Models.DTOs.MedicalRecord;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("3.0")]

    public class MedicalRecordController(IMedicalRecordService medicalRecordService) : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService = medicalRecordService;

        [HttpGet("AllMedicalRecords")]
        [Authorize(Roles = "SuperAdmin, Admin, Doctor, User")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var medicationRecordList = await _medicalRecordService.GetAllAsync();
                return Ok(medicationRecordList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin, Doctor")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var medicationRecordDto = await _medicalRecordService.GetByIdAsync(id);
                return Ok(medicationRecordDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddMedicalRecord")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Add(CreateMedicalRecordDto createMedicalRecordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _medicalRecordService.AddAsync(createMedicalRecordDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalRecordDto updateMedicalRecordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _medicalRecordService.UpdateAsync(id, updateMedicalRecordDto);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _medicalRecordService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }

        [HttpPut("AddDiagnosisAndPrescriptions")]
        [Authorize(Roles = "SuperAdmin, Admin, Doctor")]
        public async Task<IActionResult> AddDiagnosisAndPrescriptions(int recordId, string diagnosis, string prescriptions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _medicalRecordService.UpdateDiagnosisAndPrescriptions(recordId, diagnosis, prescriptions);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpGet("GetMedicalHistoryByPatientAndDoctor/{patientId}/{doctorId}")]
        [Authorize(Roles = "SuperAdmin, Admin, Doctor, User")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetMedicalHistoryByPatientIdAndDoctorId(int patientId, int doctorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var history = await _medicalRecordService.GetMedicalHistoryByPatientIdAndDoctorId(patientId, doctorId);
                return Ok(history);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the medical history.");
            }
        }

        [HttpGet("Filtering")]
        [Authorize(Roles = "SuperAdmin, Admin, Doctor")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetFilteredMedicalRecords([FromQuery] MedicalRecordFilterDto filterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var medicalRecords = await _medicalRecordService.GetFilteredMedicalRecordsAsync(filterDto);

                return Ok(medicalRecords);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
