using MedicalSystemApi.Models.DTOs.Auth;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class AppAdminController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AppAdminController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _authService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _authService.GetUserByEmailAsync(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserById/{Id}")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            try
            {
                var user = await _authService.GetUserByIdAsync(Id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateUser/{Id}")]
        public async Task<IActionResult> UpdateUser(string Id, [FromBody] UpdateUserDto model)
        {
            try
            {
                await _authService.UpdateUserAsync(Id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _authService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsersByRole/{roleName}")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            try
            {
                var users = await _authService.GetUsersByRoleAsync(roleName);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
