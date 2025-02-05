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

        public async Task<IEnumerable<Medication>> GetMedicationsByDosageRangeAsync(string minDosage, string maxDosage)
        {
            if (string.IsNullOrEmpty(minDosage) || string.IsNullOrEmpty(maxDosage))
                throw new ArgumentException("Dosage values cannot be null or empty");

            // Try to extract numeric values from the Dosage strings (e.g., "10 mg" -> 10)
            if (!decimal.TryParse(minDosage.Split(' ')[0], out decimal minDosageValue) ||
                !decimal.TryParse(maxDosage.Split(' ')[0], out decimal maxDosageValue))
            {
                throw new ArgumentException("Invalid dosage format. Please use a valid format like '10 mg'");
            }
            var medications = await _dbContext.Medications.ToListAsync();

            return medications
                .Where(m =>
                    decimal.TryParse(m.Dosage.Split(' ')[0], out decimal dosage) &&
                    dosage >= minDosageValue && dosage <= maxDosageValue)
                .ToList();
        }

        public async Task<Dictionary<string, int>> GetMedicationStatisticsAsync()
        {
            return await _dbContext.Medications
           .GroupBy(m => m.Route)
           .Select(g => new { Route = g.Key, Count = g.Count() })
           .ToDictionaryAsync(x => x.Route, x => x.Count);
        }

        public async Task<bool> MedicalRecordIdExistsAsync(int MedicalRecordId)
        {
            if (MedicalRecordId <= 0)
                throw new ArgumentException("Invalid Medical Record ID.");

            return await _dbContext.MedicalRecords.AnyAsync(mr => mr.Id == MedicalRecordId);

        }

        public async Task<bool> UpdateMedicationInstructionsAsync(int id, string instructions)
        {
            var medication = await _dbContext.Medications.FindAsync(id);
            if (medication == null)
                return false;

            medication.Instructions = instructions;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
