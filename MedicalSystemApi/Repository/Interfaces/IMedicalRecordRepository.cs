using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        Task<IEnumerable<MedicalRecord>> GetAllWithDoctorName();
        Task<MedicalRecord> GetMedicalRecordWithDoctorName(int id);
        Task<bool> PatientIdExistsAsync(int id);
        Task<bool> DoctorIdExistsAsync(int id);
    }
}
