using MedicalSystemApi.Models.DTOs.Staff;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> AllStaff()
        {
            try
            {
                var staff = await _staffService.GetAllAsync();
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

                var staffDto = await _staffService.GetByIdAsync(id);
                return Ok(staffDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddStaff")]
        public async Task<IActionResult> Add(CreateStaffDto createStaffDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _staffService.AddAsync(createStaffDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStaffDto updateStaffDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _staffService.UpdateAsync(id, updateStaffDto);
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
                await _staffService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }
    }
}
