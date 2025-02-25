using MedicalSystemApi.Models.DTOs.BillMedicalTest;
using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IBillMedicalTestRepository : IGenericRepository<BillMedicalTest>
    {
        Task<IEnumerable<BillMedicalTestDto>> GetAllWithMedicalTestName();
        Task<BillMedicalTestDto> GetOneWithMedicalTestName(int id);
        Task<bool> BillExistsAsync(int billId);
        Task<bool> MedicalTestExistsAsync(int medicalTestId);

        Task<IEnumerable<BillMedicalTestDto>> GetTestsByBillIdAsync(int billId);
        Task<IEnumerable<Bill>> GetBillsByTestIdAsync(int testId);
        Task<bool> UpdateTestCostAsync(int id, decimal newCost);

        Task<IEnumerable<BillMedicalTestDto>> GetFilteredBillMedicalTestsAsync(BillMedicalTestFilterDto filterDto);
    }
}
