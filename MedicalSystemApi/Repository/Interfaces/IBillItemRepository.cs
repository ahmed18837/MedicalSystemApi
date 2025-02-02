using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IBillItemRepository : IGenericRepository<BillItem>
    {
        Task<bool> BillExistsAsync(int billId);
    }
}
