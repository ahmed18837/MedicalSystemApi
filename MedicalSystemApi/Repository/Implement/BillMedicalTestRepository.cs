using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class BillMedicalTestRepository : GenericRepository<BillMedicalTest>, IBillMedicalTestRepository
    {
        private readonly AppDbContext _dbContext;
        public BillMedicalTestRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BillMedicalTest>> GetAllWithMedicalTestName()
        {
            return await _dbContext.BillMedicalTests
                //.AsNoTrackingWithIdentityResolution()
                .AsNoTracking()
                .Include(m => m.MedicalTest)
                .ToListAsync();
        }

        public async Task<BillMedicalTest> GetOneWithMedicalTestName(int id)
        {
            return await _dbContext.BillMedicalTests
           //.AsNoTrackingWithIdentityResolution()
           .AsNoTracking()
           .Include(m => m.MedicalTest)
           .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> MedicalTestExistsAsync(int medicalTestId)
        {
            return await _dbContext.MedicalTests
                 .AnyAsync(d => d.Id == medicalTestId);
        }

        public async Task<bool> BillExistsAsync(int billId)
        {
            return await _dbContext.Bills
                .AnyAsync(d => d.Id == billId);
        }

        public async Task<IEnumerable<BillMedicalTest>> GetTestsByBillIdAsync(int billId)
        {
            return await _dbContext.BillMedicalTests
            .Where(t => t.BillId == billId)
            .AsNoTracking()
            .Include(t => t.MedicalTest)
            .ToListAsync();
        }

        public async Task<IEnumerable<Bill>> GetBillsByTestIdAsync(int testId)
        {
            return await _dbContext.BillMedicalTests
           .Where(t => t.MedicalTestId == testId)
           .AsNoTracking()
           .Select(t => t.Bill)
           .ToListAsync();
        }

        public async Task<bool> UpdateTestCostAsync(int id, decimal newCost)
        {
            var billMedicalTest = await _dbContext.BillMedicalTests.FindAsync(id);
            if (billMedicalTest == null)
                return false; // Entry not found

            billMedicalTest.TestCost = newCost;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
