using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class MedicalTestRepository(AppDbContext dbContext) : GenericRepository<MedicalTest>(dbContext), IMedicalTestRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

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

        public async Task<IEnumerable<MedicalTest>> GetFilteredMedicalTestsAsync(MedicalTestFilterDto filterDto)
        {
            var query = _dbContext.MedicalTests.AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.TestName))
                query = query.Where(m => EF.Functions.Like(m.TestName, $"%{filterDto.TestName}%"));

            if (filterDto.MinCost.HasValue)
                query = query.Where(m => m.Cost >= filterDto.MinCost.Value);

            if (filterDto.MaxCost.HasValue)
                query = query.Where(m => m.Cost <= filterDto.MaxCost.Value);

            return await query.ToListAsync();
        }
    }
}
