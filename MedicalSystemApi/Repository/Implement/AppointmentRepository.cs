using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.Appointment;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace MedicalSystemApi.Repository.Implement
{
    public class AppointmentRepository(AppDbContext dbContext) : GenericRepository<Appointment>(dbContext), IAppointmentRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<AppointmentDto>> GetAllWithDoctorAndPatientAndStaff()
        {
            return await _dbContext.Appointments
        .AsNoTrackingWithIdentityResolution()
        .Select(a => new AppointmentDto
        {
            //Id = a.Id,
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
            //Id = a.Id,
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

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _dbContext.Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId)
                .Select(a => new AppointmentDto
                {
                    //Id = a.Id,
                    Date = a.Date,
                    Time = a.Time,
                    Status = a.Status,
                    Notes = a.Notes,
                    DoctorName = _dbContext.Doctors
                        .Where(d => d.Id == a.DoctorId)
                        .Select(d => d.FullName)
                        .FirstOrDefault(),
                    StaffName = _dbContext.Staffs
                        .Where(s => s.Id == a.StaffId)
                        .Select(s => s.FullName)
                        .FirstOrDefault(),
                    PatientName = _dbContext.Patients
                        .Where(s => s.Id == a.PatientId)
                        .Select(s => s.FullName)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return await _dbContext.Appointments
               .AsNoTracking()
               .Where(a => a.DoctorId == doctorId)
               .Select(a => new AppointmentDto
               {
                   //Id = a.Id,
                   Date = a.Date,
                   Time = a.Time,
                   Status = a.Status,
                   Notes = a.Notes,
                   DoctorName = _dbContext.Patients
                       .Where(d => d.Id == a.PatientId)
                       .Select(d => d.FullName)
                       .FirstOrDefault(),
                   StaffName = _dbContext.Staffs
                       .Where(s => s.Id == a.StaffId)
                       .Select(s => s.FullName)
                       .FirstOrDefault(),
                   PatientName = _dbContext.Patients
                        .Where(s => s.Id == a.PatientId)
                        .Select(s => s.FullName)
                        .FirstOrDefault()
               })
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

        public async Task<IEnumerable<AppointmentDto>> GetFilteredAppointmentsAsync(AppointmentFilterDto filterDto)
        {
            var query = _dbContext.Appointments
                .AsNoTracking()
                .AsQueryable();

            if (filterDto.PatientId.HasValue)
                query = query.Where(a => a.PatientId == filterDto.PatientId);

            if (filterDto.DoctorId.HasValue)
                query = query.Where(a => a.DoctorId == filterDto.DoctorId);

            if (filterDto.StaffId.HasValue)
                query = query.Where(a => a.StaffId == filterDto.StaffId);

            if (!string.IsNullOrEmpty(filterDto.Status))
                query = query.Where(a => a.Status == filterDto.Status);

            if (filterDto.MinDate.HasValue)
                query = query.Where(a => a.Date >= filterDto.MinDate.Value);

            if (filterDto.MaxDate.HasValue)
                query = query.Where(a => a.Date <= filterDto.MaxDate.Value);


            var appointments = query.Select(a => new AppointmentDto
            {
                //Id = a.Id,
                Date = a.Date,
                Time = a.Time,
                Status = a.Status,
                Notes = a.Notes,
                PatientName = _dbContext.Patients
                    .Where(p => p.Id == a.PatientId)
                    .Select(p => p.FullName)
                    .FirstOrDefault(),
                DoctorName = _dbContext.Doctors
                    .Where(d => d.Id == a.DoctorId)
                    .Select(d => d.FullName)
                    .FirstOrDefault(),
                StaffName = _dbContext.Staffs
                    .Where(s => s.Id == a.StaffId)
                    .Select(s => s.FullName)
                    .FirstOrDefault()
            });


            if (!string.IsNullOrEmpty(filterDto.OrderByField))
            {
                bool isAscending = filterDto.OrderType?.ToLower() == "asc";
                appointments = ApplySorting(appointments, filterDto.OrderByField, isAscending);
            }

            return await appointments.ToListAsync();
        }


        // Sorting Helper
        private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string orderByField, bool isAscending)
        {
            var entityType = typeof(T);
            var property = entityType.GetProperty(orderByField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return query; // No sorting if invalid field

            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { entityType, property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExpression)
            );

            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
