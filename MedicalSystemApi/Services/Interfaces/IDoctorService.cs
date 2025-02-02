using MedicalSystemApi.Models.DTOs.Doctor;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task AddAsync(CreateDoctorDto createDoctorDto);
        Task UpdateAsync(int id, UpdateDoctorDto updateDoctorDto);
        Task DeleteAsync(int id);
    }
}
