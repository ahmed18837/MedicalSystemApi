using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IStaffRepository : IGenericRepository<Staff>
    {
        Task<IEnumerable<Staff>> GetAllWithDepartmentAsync();
        Task<Staff> GetStaffWithDepartmentAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> IsEmailValid(string email);
        Task<bool> DepartmentExistsAsync(int departmentId);
        Task<bool> IsPhoneNumberValid(string phoneNumber);
        Task<bool> PhoneExistsAsync(string phoneNumber);
    }
}
