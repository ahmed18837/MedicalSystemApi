using MedicalSystemApi.Models.DTOs.MedicalRecord;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("3.0")]

    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [HttpGet("AllMedicalRecords")]
        public async Task<IActionResult> GetAll()
        {
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
        public async Task<IActionResult> Delete(int id)
        {
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
        public async Task<IActionResult> AddDiagnosisAndPrescriptions(int recordId, string diagnosis, string prescriptions)
        {
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
        public async Task<IActionResult> GetMedicalHistoryByPatientIdAndDoctorId(int patientId, int doctorId)
        {
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
    }
}
