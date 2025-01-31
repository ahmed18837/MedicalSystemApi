using MedicalSystemApi.Models.DTOs.Patient;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task AddAsync(CreatePatientDto createPatientDto);
        Task UpdateAsync(int id, UpdatePatientDto updatePatientDto);
        Task DeleteAsync(int id);

    }
}
