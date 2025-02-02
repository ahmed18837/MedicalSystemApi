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
    }
}
