using MedicalSystemApi.Models.DTOs.Staff;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffDto>> GetAllAsync();
        Task<StaffDto> GetByIdAsync(int id);
        Task AddAsync(CreateStaffDto createStaffDto);
        Task UpdateAsync(int id, UpdateStaffDto updateStaffDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<StaffDto>> GetStaffByDepartmentAsync(int departmentId);
        Task<Dictionary<string, int>> GetStaffCountByRoleAsync();
        Task<Dictionary<string, int>> GetStaffCountByDepartmentAsync();
        Task<StaffDto> SearchWithEmailStaffAsync(string phone);
        Task<StaffDto> SearchWithPhoneStaffAsync(string email);
        Task<int> GetYearsOfServiceAsync(int id);
        Task UpdateStaffRoleOrDepartmentAsync(UpdateStaffRoleOrDepartmentDto updateDto);
        Task UpdateStaffImageAsync(string staffId, IFormFile file);
    }
}
