using MedicalSystemApi.Models.DTOs.BillItem;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IBillItemService
    {
        Task<IEnumerable<BillItemDto>> GetAllAsync();
        Task<BillItemDto> GetByIdAsync(int id);
        Task AddAsync(CreateBillItemDto createBillItemDto);
        Task UpdateAsync(int id, UpdateBillItemDto updateBillItemDto);
        Task DeleteAsync(int id);

        Task<IEnumerable<BillItemDto>> GetBillItemsByBillIdAsync(int billId);
        Task UpdateBillItemPriceAsync(int billItemId, decimal unitPrice, int quantity);
    }
}
