using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MedicalSystemApi.Repository.Implement
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext _dbContext;
        public DoctorRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Doctor>> GetAllWithDepartmentName()
        {
            return await _dbContext.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .ToListAsync();
        }

        public async Task<Doctor> GetDoctorWithDepartmentName(int id)
        {
            return await _dbContext.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> DepartmentExistsAsync(int departmentId)
        {
            return await _dbContext.Departments
                .AnyAsync(d => d.Id == departmentId);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Doctors
              .AnyAsync(s => s.Email.ToUpper() == email.ToUpper());
        }

        public async Task<bool> IsEmailValid(string email)
        {
            var emailPattern = @"^[\w-\.]+@([\w-]+\.)+[a-zA-Z]{2,7}$";

            return Regex.IsMatch(email, emailPattern);
        }

        public async Task<bool> IsPhoneNumberValid(string phoneNumber)
        {
            var egyptPhonePattern = @"^\+20\d{10}$";
            return Regex.IsMatch(phoneNumber, egyptPhonePattern);
        }

        public async Task<bool> PhoneExistsAsync(string phoneNumber)
        {
            return await _dbContext.Doctors
               .AnyAsync(p => p.Phone == phoneNumber);
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecialty(string specialty)
        {
            return await _dbContext.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .Where(d => d.Specialty.ToLower() == specialty.ToLower())
                .ToListAsync(); // Return full Doctor entity
        }

        public async Task<IEnumerable<Doctor>> GetAvailableDoctorsToDay()
        {
            var today = DateTime.Now.Date;

            return await _dbContext.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .Where(d => d.Appointments!.Any(a => a.Date.Date == today))
                .ToListAsync();
        }

        public async Task<bool> AssignDoctorToDepartment(int doctorId, int departmentId)
        {
            var doctor = await _dbContext.Doctors.FindAsync(doctorId);
            if (doctor == null) return false;

            var department = await _dbContext.Departments.FindAsync(departmentId);
            if (department == null) return false;

            doctor.DepartmentId = departmentId;

            _dbContext.Doctors.Update(doctor);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDoctorWorkingHoursAsync(int doctorId, string newWorkingHours)
        {
            var doctor = await _dbContext.Doctors.FindAsync(doctorId);
            if (doctor == null) return false; // Doctor not found

            doctor.WorkingHours = newWorkingHours;
            _dbContext.Doctors.Update(doctor);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
