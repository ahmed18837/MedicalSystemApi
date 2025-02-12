using MedicalSystemApi.Models.DTOs.Appointment;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }


        [HttpGet("AllAppointments")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var appointmentList = await _appointmentService.GetAllAsync();
                return Ok(appointmentList);
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

                var appointmentDto = await _appointmentService.GetByIdAsync(id);
                return Ok(appointmentDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto appointmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _appointmentService.CreateAppointmentAsync(appointmentDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto updateAppointmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _appointmentService.UpdateAsync(id, updateAppointmentDto);
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
                await _appointmentService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }


        [HttpGet("GetAppointmentsByPatient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateStatus/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, [FromBody] string newStatus)
        {
            try
            {
                await _appointmentService.UpdateAppointmentStatusAsync(appointmentId, newStatus);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAppointmentsByDoctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUpcomingAppointments")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetUpcomingAppointmentsAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CheckDoctorAvailability")]
        public async Task<IActionResult> CheckDoctorAvailability(int doctorId, DateTime date, TimeSpan time)
        {
            try
            {
                var availabilityMessage = await _appointmentService.CheckDoctorAvailabilityAsync(doctorId, date, time);
                return Ok(availabilityMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
