using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        private readonly AppDbContext _dbContext;
        public MedicalRecordRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MedicalRecord>> GetAllWithDoctorName()
        {
            return await _dbContext.MedicalRecords
                 .AsNoTracking()
                 .Include(d => d.Doctor)
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

        public async Task<MedicalRecord> GetMedicalRecordWithDoctorName(int id)
        {
            return await _dbContext.MedicalRecords
            .AsNoTracking() // يقلل من تحميل البيانات غير الضرورية
            .Include(s => s.Doctor) // لا يزال يجلب كل بيانات القسم، لكن بدون تتبعها
            .FirstOrDefaultAsync(s => s.Id == id);
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
    }
}
