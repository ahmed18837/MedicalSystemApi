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
    }
}
