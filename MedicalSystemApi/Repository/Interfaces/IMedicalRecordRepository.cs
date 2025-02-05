using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        Task<IEnumerable<MedicalRecord>> GetAllWithDoctorName();
        Task<MedicalRecord> GetMedicalRecordWithDoctorName(int id);
        Task<bool> PatientIdExistsAsync(int id);
        Task<bool> DoctorIdExistsAsync(int id);

        Task<bool> UpdateDiagnosisAndPrescriptions(int recordId, string diagnosis, string prescriptions);
        Task<IEnumerable<MedicalRecord>> GetMedicalHistoryByPatientIdAndDoctorId(int patientId, int doctorId);
    }
}
