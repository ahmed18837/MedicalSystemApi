using MedicalSystemApi.Models.DTOs.MedicalTest;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IMedicalTestService
    {
        Task<IEnumerable<MedicalTestDto>> GetAllAsync();
        Task<MedicalTestDto> GetByIdAsync(int id);
        Task AddAsync(CreateMedicalTestDto createMedicalTestDto);
        Task UpdateAsync(int id, UpdateMedicalTestDto updateMedicalTestDto);
        Task DeleteAsync(int id);
    }
}
