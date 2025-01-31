using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using System.Text.RegularExpressions;

namespace MedicalSystemApi.Repository.Implement
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsPhoneNumberValid(string phoneNumber)
        {
            var egyptPhonePattern = @"^\+20\d{10}$";
            return Regex.IsMatch(phoneNumber, egyptPhonePattern);
        }
    }
}
