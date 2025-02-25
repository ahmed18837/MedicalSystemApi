using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.MedicalRecord;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class MedicalRecordRepository(AppDbContext dbContext) : GenericRepository<MedicalRecord>(dbContext), IMedicalRecordRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<MedicalRecordDto>> GetAllWithDoctorName()
        {
            return await _dbContext.MedicalRecords
                 .AsNoTrackingWithIdentityResolution()
                 .Select(m => new MedicalRecordDto
                 {
                     CreatedAt = m.CreatedAt,
                     Diagnosis = m.Diagnosis,
                     Prescriptions = m.Prescriptions,
                     AdditionalNotes = m.AdditionalNotes,
                     PatientId = m.PatientId,
                     DoctorName = m.Doctor.FullName
                 })
                 .ToListAsync();
        }

        public async Task<bool> PatientIdExistsAsync(int id)
        {
            return await _dbContext.MedicalRecords
                .AnyAsync(d => d.PatientId == id);
        }

        public async Task<bool> DoctorIdExistsAsync(int id)
        {
            return await _dbContext.MedicalRecords
                 .AnyAsync(d => d.DoctorId == id);
        }

        public async Task<MedicalRecordDto> GetMedicalRecordWithDoctorName(int id)
        {
            return await _dbContext.MedicalRecords
            .AsNoTrackingWithIdentityResolution()
            .Where(m => m.Id == id)
                 .Select(m => new MedicalRecordDto
                 {
                     CreatedAt = m.CreatedAt,
                     Diagnosis = m.Diagnosis,
                     Prescriptions = m.Prescriptions,
                     AdditionalNotes = m.AdditionalNotes,
                     PatientId = m.PatientId,
                     DoctorName = m.Doctor.FullName
                 })
            .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateDiagnosisAndPrescriptions(int recordId, string diagnosis, string prescriptions)
        {
            var record = await _dbContext.MedicalRecords.FindAsync(recordId);
            if (record == null) return false;

            record.Diagnosis = diagnosis;
            record.Prescriptions = prescriptions;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalHistoryByPatientIdAndDoctorId(int patientId, int doctorId)
        {
            return await _dbContext.MedicalRecords
                .Where(r => r.PatientId == patientId && r.DoctorId == doctorId)
                .Include(r => r.Doctor)
                .Include(r => r.Medications)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetFilteredMedicalRecordsAsync(MedicalRecordFilterDto filterDto)
        {
            var query = _dbContext.MedicalRecords
                .AsQueryable();

            if (filterDto.PatientId.HasValue)
                query = query.Where(mr => mr.PatientId == filterDto.PatientId.Value);

            if (filterDto.DoctorId.HasValue)
                query = query.Where(mr => mr.DoctorId == filterDto.DoctorId.Value);

            if (!string.IsNullOrEmpty(filterDto.Diagnosis))
                query = query.Where(mr => EF.Functions.Like(mr.Diagnosis, $"%{filterDto.Diagnosis}%"));

            if (filterDto.StartDate.HasValue)
                query = query.Where(mr => mr.CreatedAt >= filterDto.StartDate.Value);

            if (filterDto.EndDate.HasValue)
                query = query.Where(mr => mr.CreatedAt <= filterDto.EndDate.Value);

            return await query
        .Select(mr => new MedicalRecordDto
        {
            CreatedAt = mr.CreatedAt,
            Diagnosis = mr.Diagnosis,
            Prescriptions = mr.Prescriptions,
            AdditionalNotes = mr.AdditionalNotes,
            PatientId = mr.PatientId,
            DoctorName = _dbContext.Doctors
                .Where(d => d.Id == mr.DoctorId)
                .Select(d => d.FullName)
                .FirstOrDefault() // جلب اسم الطبيب بدون Include
        })
        .ToListAsync();
        }

    }
}
