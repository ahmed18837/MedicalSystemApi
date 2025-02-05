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

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _dbContext.Appointments
           .AsNoTracking()
           .Where(a => a.PatientId == patientId)
           .Include(a => a.Patient) // Include Patient entity
           .Include(a => a.Doctor)  // Include Doctor entity
           .Include(a => a.Staff)   // Include Staff entity
           .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return await _dbContext.Appointments
           .AsNoTracking()
           .Where(a => a.DoctorId == doctorId)
            .Include(a => a.Patient) // Include Patient entity
           .Include(a => a.Doctor)  // Include Doctor entity
           .Include(a => a.Staff)   // Include Staff entity
           .ToListAsync();
        }

        public async Task<bool> CheckDoctorAvailabilityAsync(int doctorId, DateTime date, TimeSpan time)
        {
            return await _dbContext.Appointments
            .AnyAsync(a => a.DoctorId == doctorId && a.Date == date && a.Time == time && a.Status != "Cancelled");


        }

        public async Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status)
        {
            var appointment = await _dbContext.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                return false;

            appointment.Status = status;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync()
        {
            return await _dbContext.Appointments
           .Where(a => a.Date >= DateTime.Today)
           .OrderBy(a => a.Date)
           .ThenBy(a => a.Time)
           .ToListAsync();
        }


    }
}
