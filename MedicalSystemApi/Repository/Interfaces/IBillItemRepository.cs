using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IBillItemRepository : IGenericRepository<BillItem>
    {
        Task<bool> BillExistsAsync(int billId);

        Task<IEnumerable<BillItem>> GetBillItemsByBillIdAsync(int billId);
        Task<bool> UpdatePriceBasedOnQuantityAsync(int billItemId, decimal unitPrice, int quantity);

    }
}
