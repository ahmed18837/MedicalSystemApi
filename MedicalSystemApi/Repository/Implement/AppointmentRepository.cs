using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.Appointment;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _dbContext;
        public AppointmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllWithDoctorAndPatientAndStaff()
        {
            return await _dbContext.Appointments
        .AsNoTrackingWithIdentityResolution()
        .Select(a => new AppointmentDto
        {
            Id = a.Id,
            Date = a.Date,
            Time = a.Time,
            Status = a.Status,
            Notes = a.Notes,
            PatientName = a.Patient.FullName,
            DoctorName = a.Doctor.FullName,
            StaffName = a.Staff.FullName
        })
        .ToListAsync();
        }

        public async Task<AppointmentDto> GetByIdWithDoctorAndPatientAndStaff(int id)
        {
            return await _dbContext.Appointments
        .AsNoTrackingWithIdentityResolution()
        .Where(a => a.Id == id)
        .Select(a => new AppointmentDto
        {
            Id = a.Id,
            Date = a.Date,
            Time = a.Time,
            Status = a.Status,
            Notes = a.Notes,
            PatientName = a.Patient.FullName,
            DoctorName = a.Doctor.FullName,
            StaffName = a.Staff.FullName
        })
        .FirstOrDefaultAsync();
        }

        public async Task<bool> PatientExistsAsync(int? patientId)
        {
            return await _dbContext.Patients
              .AnyAsync(i => i.Id == patientId);
        }

        public async Task<bool> DoctorExistsAsync(int? doctorId)
        {
            return await _dbContext.Doctors
              .AnyAsync(i => i.Id == doctorId);
        }

        public async Task<bool> StaffExistsAsync(int? staffId)
        {
            return await _dbContext.Staffs
              .AnyAsync(i => i.Id == staffId);
        }
    }
}
