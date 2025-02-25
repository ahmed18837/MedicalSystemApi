using MedicalSystemApi.Models.DTOs.Patient;
using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<bool> IsPhoneNumberValid(string phoneNumber);
        Task<IEnumerable<Patient>> GetPatientsByGenderAsync(string gender);
        Task<IEnumerable<Patient>> GetPatientsByAgeRangeAsync(int minAge, int maxAge);
        Task<IEnumerable<Patient>> GetPatientsWithAppointmentsAsync();
        Task<Dictionary<string, int>> GetPatientCountByGenderAsync();
        Task<IEnumerable<Patient>> GetPatientsAdmittedInLastYearAsync(int year);
        Task<IEnumerable<Patient>> SearchPatientsByNameAsync(string name);
        Task<bool> UpdatePatientPhoneAsync(int patientId, string newPhone);
        // Filtering
        Task<IEnumerable<Patient>> GetFilteredPatientsAsync(PatientFilterDto filterDto);
    }
}
