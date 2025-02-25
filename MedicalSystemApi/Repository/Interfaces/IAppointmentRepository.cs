using MedicalSystemApi.Models.DTOs.Appointment;
using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<AppointmentDto>> GetAllWithDoctorAndPatientAndStaff();
        Task<AppointmentDto> GetByIdWithDoctorAndPatientAndStaff(int id);
        Task<bool> PatientExistsAsync(int? patientId);
        Task<bool> DoctorExistsAsync(int? doctorId);
        Task<bool> StaffExistsAsync(int? staffId);

        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<bool> CheckDoctorAvailabilityAsync(int doctorId, DateTime date, TimeSpan time);
        Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status);

        Task<IEnumerable<AppointmentDto>> GetFilteredAppointmentsAsync(AppointmentFilterDto filterDto);
    }
}
