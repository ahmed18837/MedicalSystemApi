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

        public async Task<IEnumerable<BillItem>> GetBillItemsByBillIdAsync(int billId)
        {
            return await _dbContext.BillItems
            .Where(b => b.BillId == billId)
            .ToListAsync();
        }

        public async Task<bool> UpdatePriceBasedOnQuantityAsync(int billItemId, decimal unitPrice, int quantity)
        {
            var billItem = await _dbContext.BillItems.FindAsync(billItemId);

            if (billItem == null)
                return false;

            // Update price and quantity
            billItem.Price = unitPrice;
            billItem.Quantity = quantity;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
