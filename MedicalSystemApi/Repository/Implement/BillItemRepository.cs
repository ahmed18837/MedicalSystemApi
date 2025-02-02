using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class BillItemRepository : GenericRepository<BillItem>, IBillItemRepository
    {
        private readonly AppDbContext _dbContext;
        public BillItemRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> BillExistsAsync(int billId)
        {
            return await _dbContext.Bills
               .AnyAsync(i => i.Id == billId);
        }
    }
}
