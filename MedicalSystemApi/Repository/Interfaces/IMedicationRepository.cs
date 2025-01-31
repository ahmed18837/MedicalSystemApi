using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IMedicationRepository : IGenericRepository<Medication>
    {
        Task<bool> MedicalRecordIdExistsAsync(int MedicalRecordId);
    }
}
