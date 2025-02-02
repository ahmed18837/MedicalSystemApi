using MedicalSystemApi.Models.DTOs.Appointment;
using MedicalSystemApi.Services.Interfaces;
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

        [HttpPost("AddAppointment")]
        public async Task<IActionResult> Add(CreateAppointmentDto createAppointmentDto)
        {
            //try
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }

            await _appointmentService.AddAsync(createAppointmentDto);
            return StatusCode(201);
        }
        //catch (Exception ex)
        //{
        //    return BadRequest(ex.Message);
        //}


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
    }
}
