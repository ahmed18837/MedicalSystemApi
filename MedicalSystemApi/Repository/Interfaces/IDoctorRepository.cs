using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetAllWithDepartmentName();
        Task<Doctor> GetDoctorWithDepartmentName(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> IsEmailValid(string email);
        Task<bool> DepartmentExistsAsync(int departmentId);
        Task<bool> IsPhoneNumberValid(string phoneNumber);
        Task<bool> PhoneExistsAsync(string phoneNumber);

        Task<IEnumerable<Doctor>> GetDoctorsBySpecialty(string specialty);
        Task<IEnumerable<Doctor>> GetAvailableDoctorsToDay();

        Task<bool> AssignDoctorToDepartment(int doctorId, int departmentId);
        Task<bool> UpdateDoctorWorkingHoursAsync(int doctorId, string newWorkingHours);
    }
}
