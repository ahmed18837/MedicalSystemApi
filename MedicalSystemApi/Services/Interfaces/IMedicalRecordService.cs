using MedicalSystemApi.Models.DTOs.MedicalRecord;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecordDto>> GetAllAsync();
        Task<MedicalRecordDto> GetByIdAsync(int id);
        Task AddAsync(CreateMedicalRecordDto createMedicalRecordDto);
        Task UpdateAsync(int id, UpdateMedicalRecordDto updateMedicalRecordDto);
        Task DeleteAsync(int id);

        Task UpdateDiagnosisAndPrescriptions(int recordId, string diagnosis, string prescriptions);
        Task<IEnumerable<MedicalRecordDto>> GetMedicalHistoryByPatientIdAndDoctorId(int patientId, int doctorId);

        Task<IEnumerable<MedicalRecordDto>> GetFilteredMedicalRecordsAsync(MedicalRecordFilterDto filterDto);
    }
}
