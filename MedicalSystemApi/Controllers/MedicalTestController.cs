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

        [HttpGet("GetExpensiveTests")]
        public async Task<IActionResult> GetExpensiveTests(decimal minCost)
        {
            try
            {
                var tests = await _medicalTestService.GetExpensiveTests(minCost);
                return Ok(tests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchMedicalTests(string searchTerm)
        {
            try
            {
                var results = await _medicalTestService.SearchMedicalTests(searchTerm);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AssignTestToBill")]
        public async Task<IActionResult> AssignTestToBill(int testId, int billId)
        {
            try
            {
                await _medicalTestService.AssignMedicalTestToBill(testId, billId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while assigning test.");
            }
        }

        [HttpPut("UpdateCost")]
        public async Task<IActionResult> UpdateCost(int testId, decimal newCost)
        {
            try
            {
                await _medicalTestService.UpdateMedicalTestCost(testId, newCost);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating cost.");
            }
        }

    }
}
