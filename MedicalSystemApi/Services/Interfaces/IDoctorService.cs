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

        Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialty(string specialty);
        Task<IEnumerable<DoctorDto>> GetAvailableDoctorsToDay();

        Task AssignDoctorToDepartmentAsync(int doctorId, int departmentId);
        Task UpdateDoctorWorkingHoursAsync(int doctorId, string newWorkingHours);
    }
}
