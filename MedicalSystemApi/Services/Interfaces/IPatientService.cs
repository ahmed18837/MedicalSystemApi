using MedicalSystemApi.Models.DTOs.Patient;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task AddAsync(CreatePatientDto createPatientDto);
        Task UpdateAsync(int id, UpdatePatientDto updatePatientDto);
        Task DeleteAsync(int id);

        Task<IEnumerable<PatientDto>> GetPatientsByGenderAsync(string gender);
        Task<IEnumerable<PatientDto>> GetPatientsByAgeRangeAsync(int minAge, int maxAge);
        Task<IEnumerable<PatientDto>> GetPatientsWithAppointmentsAsync();
        Task<Dictionary<string, int>> GetPatientCountByGenderAsync();
        Task<IEnumerable<PatientDto>> GetPatientsAdmittedInLastYearAsync(int year);
        Task<IEnumerable<PatientDto>> SearchPatientsByNameAsync(string name);
        Task UpdatePatientPhoneAsync(int patientId, string newPhone);
        // Filtering
        Task<IEnumerable<PatientDto>> GetFilteredPatientsAsync(PatientFilterDto filterDto);
    }
}
