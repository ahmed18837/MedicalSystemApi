using MedicalSystemApi.Models.DTOs.Bill;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IBillService
    {
        Task<IEnumerable<BillDto>> GetAllAsync();
        Task<BillDto> GetByIdAsync(int id);
        Task AddAsync(CreateBillDto createBillDto);
        Task UpdateAsync(int id, UpdateBillDto updateBillDto);
        Task DeleteAsync(int id);

        Task<BillDto> GetBillsByPatientIdAsync(int patientId);
        Task UpdateTotalAmountAsync(int billId, decimal amount);
    }
}
