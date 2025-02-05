using MedicalSystemApi.Models.DTOs.Department;
using MedicalSystemApi.Models.DTOs.Doctor;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto> GetByIdAsync(int id);
        Task AddAsync(CreateDepartmentDto createDepartmentDto);
        Task UpdateAsync(int id, UpdateDepartmentDto updateDepartmentDto);
        Task DeleteAsync(int id);

        Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentIdAsync(int departmentId);
        Task RemoveDoctorFromDepartmentAsync(int departmentId, int doctorId);
    }
}
