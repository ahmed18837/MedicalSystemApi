using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.Doctor;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MedicalSystemApi.Repository.Implement
{
    public class DoctorRepository(AppDbContext dbContext) : GenericRepository<Doctor>(dbContext), IDoctorRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<DoctorDto>> GetAllWithDepartmentName()
        {
            return await _dbContext.Doctors
                .AsNoTrackingWithIdentityResolution()
                .Select(d => new DoctorDto
                {
                    //Id = d.Id,
                    FullName = d.FullName,
                    Age = d.Age,
                    Gender = d.Gender,
                    Phone = d.Phone,
                    Email = d.Email,
                    Specialty = d.Specialty,
                    WorkingHours = d.WorkingHours,
                    DepartmentName = d.Department.Name
                }).ToListAsync();
        }

        public async Task<DoctorDto> GetDoctorWithDepartmentName(int id)
        {
            return await _dbContext.Doctors
                .AsNoTrackingWithIdentityResolution()
                .Where(d => d.Id == id)
                .Select(d => new DoctorDto
                {
                    //Id = d.Id,
                    FullName = d.FullName,
                    Age = d.Age,
                    Gender = d.Gender,
                    Phone = d.Phone,
                    Email = d.Email,
                    Specialty = d.Specialty,
                    WorkingHours = d.WorkingHours,
                    DepartmentName = d.Department.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            return await _dbContext.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => new DoctorDto
                {
                    FullName = d.FullName,
                    Age = d.Age,
                    Gender = d.Gender,
                    Phone = d.Phone,
                    Email = d.Email,
                    Specialty = d.Specialty,
                    WorkingHours = d.WorkingHours,
                    DepartmentName = d.Department.Name, // جلب اسم القسم مباشرةً
                    ImagePath = d.ImagePath
                })
                .ToListAsync();
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

        public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialty(string specialty)
        {
            return await _dbContext.Doctors
                .AsNoTrackingWithIdentityResolution()
                .Where(d => d.Specialty.ToLower() == specialty.ToLower())
                .Select(d => new DoctorDto
                {
                    //Id = d.Id,
                    FullName = d.FullName,
                    Age = d.Age,
                    Gender = d.Gender,
                    Phone = d.Phone,
                    Email = d.Email,
                    Specialty = d.Specialty,
                    WorkingHours = d.WorkingHours,
                    DepartmentName = d.Department.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<DoctorDto>> GetAvailableDoctorsToDay()
        {
            var today = DateTime.Now.Date;

            return await _dbContext.Doctors
                .AsNoTracking()
                .Where(d => d.Appointments!.Any(a => a.Date.Date == today))
                .Select(d => new DoctorDto
                {
                    //Id = d.Id,
                    FullName = d.FullName,
                    Age = d.Age,
                    Gender = d.Gender,
                    Phone = d.Phone,
                    Email = d.Email,
                    Specialty = d.Specialty,
                    WorkingHours = d.WorkingHours,
                    DepartmentName = d.Department.Name
                })
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

        public async Task<IEnumerable<DoctorDto>> GetFilteredDoctorsAsync(DoctorFilterDto filterDto)
        {
            var query = _dbContext.Doctors
                .AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.FullName))
                query = query.Where(d => EF.Functions.Like(d.FullName, $"%{filterDto.FullName}%"));

            if (filterDto.MinAge.HasValue)
                query = query.Where(d => d.Age >= filterDto.MinAge.Value);

            if (filterDto.MaxAge.HasValue)
                query = query.Where(d => d.Age <= filterDto.MaxAge.Value);

            if (!string.IsNullOrEmpty(filterDto.Gender))
                query = query.Where(d => EF.Functions.Like(d.Gender, $"%{filterDto.Gender}%"));

            if (!string.IsNullOrEmpty(filterDto.Specialty))
                query = query.Where(d => EF.Functions.Like(d.Specialty, $"%{filterDto.Specialty}%"));

            if (filterDto.DepartmentId.HasValue)
                query = query.Where(d => d.DepartmentId == filterDto.DepartmentId.Value);

            return await query
         .Select(d => new DoctorDto
         {
             FullName = d.FullName,
             Age = d.Age,
             Gender = d.Gender,
             Phone = d.Phone,
             Email = d.Email,
             Specialty = d.Specialty,
             WorkingHours = d.WorkingHours,
             DepartmentName = _dbContext.Departments
                 .Where(dep => dep.Id == d.DepartmentId)
                 .Select(dep => dep.Name)
                 .FirstOrDefault(), // ✅ جلب اسم القسم بدون Include
             ImagePath = d.ImagePath ?? string.Empty
         })
         .ToListAsync();
        }
    }
}
