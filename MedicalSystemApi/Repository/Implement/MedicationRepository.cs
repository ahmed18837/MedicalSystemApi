using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class MedicationRepository : GenericRepository<Medication>, IMedicationRepository
    {
        private readonly AppDbContext _dbContext;
        public MedicationRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> MedicalRecordIdExistsAsync(int MedicalRecordId)
        {
            return _dbContext.MedicalRecords
                 .AnyAsync(m => m.Id == MedicalRecordId);
        }
    }
}
