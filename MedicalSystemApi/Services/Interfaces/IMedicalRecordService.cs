using MedicalSystemApi.Models.DTOs.MedicalRecord;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecordDto>> GetAllAsync();
        Task<MedicalRecordDto> GetByIdAsync(int id);
        Task AddAsync(CreateMedicalRecordDto createMedicalRecordDto);
        Task UpdateAsync(int id, UpdateMedicalRecordDto updateMedicalRecordDto);
        Task DeleteAsync(int id);
    }
}
