using MedicalSystemApi.Models.DTOs.Appointment;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task AddAsync(CreateAppointmentDto createAppointmentDto);
        Task UpdateAsync(int id, UpdateAppointmentDto updateAppointmentDto);
        Task DeleteAsync(int id);
    }
}
