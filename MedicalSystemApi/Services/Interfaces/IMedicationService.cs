using MedicalSystemApi.Models.DTOs.Medication;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IMedicationService
    {
        Task<IEnumerable<MedicationDto>> GetAllAsync();
        Task<MedicationDto> GetByIdAsync(int id);
        Task AddAsync(CreateMedicationDto createMedicationDto);
        Task UpdateAsync(int id, UpdateMedicationDto updateMedicationDto);
        Task DeleteAsync(int id);


        Task<IEnumerable<MedicationDto>> GetMedicationsByDosageRangeAsync(string minDosage, string maxDosage);
        Task UpdateMedicationInstructionsAsync(int id, string instructions);
        Task<Dictionary<string, int>> GetMedicationStatisticsAsync();

    }
}
