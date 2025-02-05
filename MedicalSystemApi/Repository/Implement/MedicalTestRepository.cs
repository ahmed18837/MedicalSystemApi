using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class MedicalTestRepository : GenericRepository<MedicalTest>, IMedicalTestRepository
    {
        private readonly AppDbContext _dbContext;
        public MedicalTestRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AssignMedicalTestToBill(int testId, int billId)
        {
            var test = await _dbContext.MedicalTests.FindAsync(testId);
            var bill = await _dbContext.Bills.FindAsync(billId);

            if (test == null || bill == null)
                throw new ArgumentException("Invalid Test ID or Bill ID.");

            var billTest = new BillMedicalTest
            {
                BillId = billId,
                MedicalTestId = testId,
                TestCost = test.Cost // ✅ Ensure cost is set
            };

            _dbContext.BillMedicalTests.Add(billTest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicalTest>> GetExpensiveTests(decimal minCost)
        {
            return await _dbContext.MedicalTests
                .Where(mt => mt.Cost > minCost)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalTest>> SearchMedicalTests(string searchTerm)
        {
            return await _dbContext.MedicalTests
            .Where(mt => mt.TestName.Contains(searchTerm))
            .ToListAsync();
        }

        public async Task<bool> UpdateMedicalTestCost(int testId, decimal newCost)
        {
            var test = await _dbContext.MedicalTests.FindAsync(testId);

            test!.Cost = newCost;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
