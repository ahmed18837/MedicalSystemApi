using MedicalSystemApi.Models.DTOs.Staff;
using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IStaffRepository : IGenericRepository<Staff>
    {
        Task<IEnumerable<StaffDto>> GetAllWithDepartmentNameAsync();
        Task<StaffDto> GetStaffWithDepartmentAsync(int id);
        Task<IEnumerable<StaffDto>> GetStaffWithDepartmentIdAsync(int departmentId);
        Task<bool> StaffIdExistsAsync(int staffId);
        Task<bool> RoleStaffExistsAsync(string roleStaff);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> IsEmailValid(string email);
        Task<bool> DepartmentExistsAsync(int departmentId);
        Task<bool> IsPhoneNumberValid(string phoneNumber);
        Task<bool> PhoneExistsAsync(string phoneNumber);

        Task<Dictionary<string, int>> GetStaffCountByRoleAsync();
        Task<Dictionary<string, int>> GetStaffCountByDepartmentAsync();
        Task<Staff> SearchWithPhoneStaffAsync(string phone);
        Task<Staff> SearchWithEmailStaffAsync(string email);
        Task<int> GetYearsOfServiceAsync(int id);
        Task UpdateStaffRoleOrDepartmentAsync(int staffId, string? roleStaff, int? departmentId);
        // Filtering
        Task<IEnumerable<Staff>> GetFilteredStaffAsync(StaffFilterDto filterDto);
    }
}
