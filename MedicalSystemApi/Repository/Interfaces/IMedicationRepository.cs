using MedicalSystemApi.Models.DTOs.Medication;
using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IMedicationRepository : IGenericRepository<Medication>
    {
        Task<bool> MedicalRecordIdExistsAsync(int MedicalRecordId);
        Task<IEnumerable<Medication>> GetMedicationsByDosageRangeAsync(string minDosage, string maxDosage);
        Task<bool> UpdateMedicationInstructionsAsync(int id, string instructions);
        Task<Dictionary<string, int>> GetMedicationStatisticsAsync();
        // Filtering
        Task<IEnumerable<Medication>> GetFilteredMedicationsAsync(MedicationFilterDto filterDto);
    }
}
