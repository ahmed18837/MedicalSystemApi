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
    }
}
