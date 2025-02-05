using MedicalSystemApi.Models.DTOs.BillMedicalTest;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillMedicalTestController : ControllerBase
    {
        private readonly IBillMedicalTestService _billMedicalTestService;

        public BillMedicalTestController(IBillMedicalTestService billMedicalTestService)
        {
            _billMedicalTestService = billMedicalTestService;
        }

        [HttpGet("AllBillMedicalTests")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var billMedicalTestList = await _billMedicalTestService.GetAllAsync();
                return Ok(billMedicalTestList);
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

                var billMedicalTestDto = await _billMedicalTestService.GetByIdAsync(id);
                return Ok(billMedicalTestDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddBillMedicalTest")]
        public async Task<IActionResult> Add(CreateBillMedicalTestDto createBillMedicalTestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _billMedicalTestService.AddAsync(createBillMedicalTestDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBillMedicalTestDto updateBillMedicalTestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _billMedicalTestService.UpdateAsync(id, updateBillMedicalTestDto);
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
                await _billMedicalTestService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }

        [HttpGet("{billId}/tests")]
        public async Task<IActionResult> GetTestsByBillId(int billId)
        {
            try
            {
                var tests = await _billMedicalTestService.GetTestsByBillIdAsync(billId);
                return Ok(tests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("test/{testId}/bills")]
        public async Task<IActionResult> GetBillsForMedicalTest(int testId)
        {
            try
            {
                var bills = await _billMedicalTestService.GetBillsForMedicalTestAsync(testId);
                return Ok(bills);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/cost")]
        public async Task<IActionResult> UpdateTestCost(int id, [FromBody] decimal newCost)
        {
            try
            {
                await _billMedicalTestService.UpdateTestCostAsync(id, newCost);
                return NoContent(); //204
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
