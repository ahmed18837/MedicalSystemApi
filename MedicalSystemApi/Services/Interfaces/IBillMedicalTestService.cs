using MedicalSystemApi.Models.DTOs.Bill;
using MedicalSystemApi.Models.DTOs.BillMedicalTest;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IBillMedicalTestService
    {
        Task<IEnumerable<BillMedicalTestDto>> GetAllAsync();
        Task<BillMedicalTestDto> GetByIdAsync(int id);
        Task AddAsync(CreateBillMedicalTestDto createBillMedicalTestDto);
        Task UpdateAsync(int id, UpdateBillMedicalTestDto updateBillMedicalTestDto);
        Task DeleteAsync(int id);

        Task<IEnumerable<BillMedicalTestDto>> GetTestsByBillIdAsync(int billId);
        Task<IEnumerable<BillDto>> GetBillsForMedicalTestAsync(int testId);
        Task UpdateTestCostAsync(int id, decimal newCost);

        Task<IEnumerable<BillMedicalTestDto>> GetFilteredBillMedicalTestsAsync(BillMedicalTestFilterDto filterDto);
    }
}
