using MedicalSystemApi.Models.DTOs.Doctor;
using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<IEnumerable<DoctorDto>> GetAllWithDepartmentName();
        Task<DoctorDto> GetDoctorWithDepartmentName(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> IsEmailValid(string email);
        Task<bool> DepartmentExistsAsync(int departmentId);
        Task<bool> IsPhoneNumberValid(string phoneNumber);
        Task<bool> PhoneExistsAsync(string phoneNumber);

        Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialty(string specialty);
        Task<IEnumerable<DoctorDto>> GetAvailableDoctorsToDay();

        Task<bool> AssignDoctorToDepartment(int doctorId, int departmentId);
        Task<bool> UpdateDoctorWorkingHoursAsync(int doctorId, string newWorkingHours);

        Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<DoctorDto>> GetFilteredDoctorsAsync(DoctorFilterDto filterDto);
    }
}
