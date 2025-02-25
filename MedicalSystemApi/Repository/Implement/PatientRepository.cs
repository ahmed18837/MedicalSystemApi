using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.Patient;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MedicalSystemApi.Repository.Implement
{
    public class PatientRepository(AppDbContext dbContext) : GenericRepository<Patient>(dbContext), IPatientRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<bool> IsPhoneNumberValid(string phoneNumber)
        {
            var egyptPhonePattern = @"^\+20\d{10}$";
            return Regex.IsMatch(phoneNumber, egyptPhonePattern);
        }

        public async Task<Dictionary<string, int>> GetPatientCountByGenderAsync()
        {
            return await _dbContext.Patients
                .GroupBy(p => p.Gender)
                .Select(g => new { Gender = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Gender, x => x.Count);
        }

        public async Task<IEnumerable<Patient>> GetPatientsAdmittedInLastYearAsync(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);
            return await _dbContext.Patients
                .Where(p => p.MedicalHistoryDate >= startDate && p.MedicalHistoryDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsByAgeRangeAsync(int minAge, int maxAge)
        {
            if (minAge < 0 || maxAge < 0 || minAge > maxAge)
                throw new ArgumentException("Invalid age range: minAge must be less than or equal to maxAge and non-negative.");

            DateTime today = DateTime.UtcNow;
            DateTime maxBirthDate = today.AddYears(-minAge); // Youngest allowed birthdate
            DateTime minBirthDate = today.AddYears(-maxAge); // Oldest allowed birthdate

            return await _dbContext.Patients
                .Where(p => p.DateOfBirth >= minBirthDate && p.DateOfBirth <= maxBirthDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsByGenderAsync(string gender)
        {
            return await _dbContext.Patients.Where(p => p.Gender == gender).ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsWithAppointmentsAsync()
        {
            return await _dbContext.Patients.Where(p => p.Appointments.Any()).ToListAsync();
        }

        public async Task<IEnumerable<Patient>> SearchPatientsByNameAsync(string name)
        {
            return await _dbContext.Patients
                .Where(p => p.FullName.Contains(name))
                .ToListAsync();
        }

        public async Task<bool> UpdatePatientPhoneAsync(int patientId, string newPhone)
        {
            var patient = await _dbContext.Patients.FindAsync(patientId);
            if (patient == null) return false;

            patient.Phone = newPhone;

            _dbContext.Patients.Update(patient);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Patient>> GetFilteredPatientsAsync(PatientFilterDto filterDto)
        {
            var query = _dbContext.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.FullName))
                query = query.Where(p => p.FullName.Contains(filterDto.FullName));

            if (!string.IsNullOrEmpty(filterDto.Gender))
                query = query.Where(p => EF.Functions.Like(p.Gender, filterDto.Gender));

            if (filterDto.DateOfBirthFrom.HasValue)
                query = query.Where(p => p.DateOfBirth >= filterDto.DateOfBirthFrom.Value);

            if (filterDto.DateOfBirthTo.HasValue)
                query = query.Where(p => p.DateOfBirth <= filterDto.DateOfBirthTo.Value);

            if (filterDto.MedicalHistoryDateFrom.HasValue)
                query = query.Where(p => p.MedicalHistoryDate >= filterDto.MedicalHistoryDateFrom.Value);

            if (filterDto.MedicalHistoryDateTo.HasValue)
                query = query.Where(p => p.MedicalHistoryDate <= filterDto.MedicalHistoryDateTo.Value);

            return await query.ToListAsync();
        }
    }
}
