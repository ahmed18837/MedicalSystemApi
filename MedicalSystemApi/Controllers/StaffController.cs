using MedicalSystemApi.Models.DTOs.Staff;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        public async Task<IActionResult> Add([FromForm] CreateStaffDto createStaffDto)
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
        public async Task<IActionResult> Update(int id, [FromForm] UpdateStaffDto updateStaffDto)
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

        [HttpGet("GetByDepartment/{departmentId}")]
        public async Task<IActionResult> GetStaffByDepartment(int departmentId)
        {
            try
            {
                var staffListDto = await _staffService.GetStaffByDepartmentAsync(departmentId);
                return Ok(staffListDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SearchWithPhone")]
        public async Task<IActionResult> SearchWithPhoneStaff([FromQuery] string phone)
        {
            try
            {
                var staff = await _staffService.SearchWithPhoneStaffAsync(phone);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SearchWithEmail")]
        public async Task<IActionResult> SearchWithEmailStaff([FromQuery] string email)
        {
            try
            {
                var staff = await _staffService.SearchWithEmailStaffAsync(email);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetYearsOfService/{id}")]
        public async Task<IActionResult> GetYearsOfService(int id)
        {
            try
            {
                var years = await _staffService.GetYearsOfServiceAsync(id);
                return Ok(new { StaffId = id, YearsOfService = years });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("countByRole")]
        public async Task<ActionResult<Dictionary<string, int>>> GetStaffCountByRole()
        {
            try
            {
                var staffCountByRole = await _staffService.GetStaffCountByRoleAsync();
                return Ok(staffCountByRole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("countByDepartment")]
        public async Task<ActionResult<Dictionary<string, int>>> GetStaffCountByDepartment()
        {
            try
            {
                var staffCountByDepartment = await _staffService.GetStaffCountByDepartmentAsync();
                return Ok(staffCountByDepartment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateRoleOrDepartment")]
        public async Task<IActionResult> UpdateStaffRoleOrDepartment([FromBody] UpdateStaffRoleOrDepartmentDto updateDto)
        {
            try
            {
                await _staffService.UpdateStaffRoleOrDepartmentAsync(updateDto);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
