using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.BillMedicalTest;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class BillMedicalTestRepository(AppDbContext dbContext) : GenericRepository<BillMedicalTest>(dbContext), IBillMedicalTestRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<BillMedicalTestDto>> GetAllWithMedicalTestName()
        {
            return await _dbContext.BillMedicalTests
         .AsNoTrackingWithIdentityResolution()
         .Select(bmt => new BillMedicalTestDto
         {
             BillId = bmt.BillId,
             MedicalTestName = _dbContext.MedicalTests
                 .Where(mt => mt.Id == bmt.MedicalTestId)
                 .Select(mt => mt.TestName)
                 .FirstOrDefault(),
             TestCost = bmt.TestCost
         })
         .ToListAsync();
        }

        public async Task<BillMedicalTestDto> GetOneWithMedicalTestName(int id)
        {
            return await _dbContext.BillMedicalTests
                .AsNoTracking() // ✅ تحسين الأداء بعدم تتبع الكيانات
                .Where(bmt => bmt.Id == id)
                .Select(bmt => new BillMedicalTestDto
                {
                    BillId = bmt.BillId,
                    MedicalTestName = _dbContext.MedicalTests
                        .Where(mt => mt.Id == bmt.MedicalTestId)
                        .Select(mt => mt.TestName)
                        .FirstOrDefault(),
                    TestCost = bmt.TestCost
                })
                .FirstOrDefaultAsync();
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

        public async Task<IEnumerable<BillMedicalTestDto>> GetTestsByBillIdAsync(int billId)
        {
            return await _dbContext.BillMedicalTests
                .Where(t => t.BillId == billId)
                .AsNoTracking()
                .Select(t => new BillMedicalTestDto
                {
                    BillId = t.BillId,
                    MedicalTestName = _dbContext.MedicalTests
                        .Where(mt => mt.Id == t.MedicalTestId)
                        .Select(mt => mt.TestName)
                        .FirstOrDefault(),
                    TestCost = t.TestCost
                })
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

        public async Task<IEnumerable<BillMedicalTestDto>> GetFilteredBillMedicalTestsAsync(BillMedicalTestFilterDto filterDto)
        {
            var query = _dbContext.BillMedicalTests.AsQueryable();

            // Filtering
            if (filterDto.BillId.HasValue)
                query = query.Where(bmt => bmt.BillId == filterDto.BillId.Value);

            if (filterDto.MedicalTestId.HasValue)
                query = query.Where(bmt => bmt.MedicalTestId == filterDto.MedicalTestId.Value);

            if (filterDto.MinTestCost.HasValue)
                query = query.Where(bmt => bmt.TestCost >= filterDto.MinTestCost.Value);

            if (filterDto.MaxTestCost.HasValue)
                query = query.Where(bmt => bmt.TestCost <= filterDto.MaxTestCost.Value);

            return await query
                .AsNoTracking()
                .Select(bmt => new BillMedicalTestDto
                {
                    BillId = bmt.BillId,
                    MedicalTestName = _dbContext.MedicalTests
                        .Where(mt => mt.Id == bmt.MedicalTestId)
                        .Select(mt => mt.TestName)
                        .FirstOrDefault(),
                    TestCost = bmt.TestCost
                })
                .ToListAsync();
        }

    }
}
