using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace MedicalSystemApi.Repository.Implement
{
    public class BillItemRepository(AppDbContext dbContext) : GenericRepository<BillItem>(dbContext), IBillItemRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

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

        public async Task<IEnumerable<BillItem>> GetFilteredBillItemsAsync(BillItemFilterDto filterDto)
        {
            var query = _dbContext.BillItems
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(filterDto.ItemName))
                query = query.Where(bi => EF.Functions.Like(bi.ItemName, $"%{filterDto.ItemName}%"));

            if (filterDto.MinPrice.HasValue)
                query = query.Where(bi => bi.Price >= filterDto.MinPrice.Value);

            if (filterDto.MaxPrice.HasValue)
                query = query.Where(bi => bi.Price <= filterDto.MaxPrice.Value);

            if (filterDto.MinQuantity.HasValue)
                query = query.Where(bi => bi.Quantity >= filterDto.MinQuantity.Value);

            if (filterDto.MaxQuantity.HasValue)
                query = query.Where(bi => bi.Quantity <= filterDto.MaxQuantity.Value);

            if (filterDto.BillId.HasValue)
                query = query.Where(bi => bi.BillId == filterDto.BillId.Value);

            // Sorting
            if (!string.IsNullOrEmpty(filterDto.OrderByField))
            {
                bool isAscending = filterDto.OrderType?.ToLower() == "asc";
                query = ApplySorting(query, filterDto.OrderByField, isAscending);
            }

            return await query.ToListAsync();
        }

        // Sorting Helper
        private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string orderByField, bool isAscending)
        {
            var entityType = typeof(T);
            var property = entityType.GetProperty(orderByField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return query; // No sorting if invalid field

            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { entityType, property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExpression)
            );

            return query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
