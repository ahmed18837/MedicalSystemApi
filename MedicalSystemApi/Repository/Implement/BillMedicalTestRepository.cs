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
    }
}
