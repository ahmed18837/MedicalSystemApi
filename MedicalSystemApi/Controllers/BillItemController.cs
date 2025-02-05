using MedicalSystemApi.Models.DTOs.BillItem;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillItemController : ControllerBase
    {
        private readonly IBillItemService _billItemService;

        public BillItemController(IBillItemService billItemService)
        {
            _billItemService = billItemService;
        }

        [HttpGet("AllBillItems")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var billItemList = await _billItemService.GetAllAsync();
                return Ok(billItemList);
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

                var billItemDto = await _billItemService.GetByIdAsync(id);
                return Ok(billItemDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddBillItem")]
        public async Task<IActionResult> Add(CreateBillItemDto createBillItemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _billItemService.AddAsync(createBillItemDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBillItemDto updateBillItemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _billItemService.UpdateAsync(id, updateBillItemDto);
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
                await _billItemService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }

        [HttpGet("bill/{billId}")]
        public async Task<IActionResult> GetBillItemsByBillId(int billId)
        {
            try
            {
                var billItems = await _billItemService.GetBillItemsByBillIdAsync(billId);
                return Ok(billItems); // 204 No Content
            }
            catch (Exception ex)
            {
                return Content(ex.Message);// 404 Not Found
            }
        }

        [HttpPut("update-price/{billItemId}")]
        public async Task<IActionResult> UpdateBillItemPrice(int billItemId, decimal unitPrice, int quantity)
        {
            try
            {
                await _billItemService.UpdateBillItemPriceAsync(billItemId, unitPrice, quantity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
