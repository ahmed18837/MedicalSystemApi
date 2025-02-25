using MedicalSystemApi.Models.DTOs.Appointment;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task CreateAppointmentAsync(CreateAppointmentDto createDto);
        Task UpdateAsync(int id, UpdateAppointmentDto updateAppointmentDto);
        Task DeleteAsync(int id);

        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<string> CheckDoctorAvailabilityAsync(int doctorId, DateTime date, TimeSpan time);
        Task UpdateAppointmentStatusAsync(int appointmentId, string status);

        Task<IEnumerable<AppointmentDto>> GetFilteredAppointmentsAsync(AppointmentFilterDto filterDto);
    }
}
