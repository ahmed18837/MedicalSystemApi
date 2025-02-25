using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.Bill;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace MedicalSystemApi.Repository.Implement
{
    public class BillRepository(AppDbContext dbContext) : GenericRepository<Bill>(dbContext), IBillRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<Bill>> GetBillsByPatientIdAsync(int patientId)
        {
            return await _dbContext.Bills
            .AsNoTracking()
            .Where(b => b.PatientId == patientId)
            .OrderByDescending(b => b.DateIssued)
            .ToListAsync();
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _dbContext.Patients
                 .AnyAsync(i => i.Id == patientId);
        }

        public async Task<bool> UpdateTotalAmountAsync(int billId, decimal amount)
        {
            var bill = await _dbContext.Bills.FindAsync(billId);
            bill!.TotalAmount = amount;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Bill>> GetFilteredBillsAsync(BillFilterDto filterDto)
        {
            var query = _dbContext.Bills.AsQueryable();

            if (filterDto.PatientId.HasValue)
                query = query.Where(b => b.PatientId == filterDto.PatientId);

            if (filterDto.MinDateIssued.HasValue)
                query = query.Where(b => b.DateIssued >= filterDto.MinDateIssued.Value);

            if (filterDto.MaxDateIssued.HasValue)
                query = query.Where(b => b.DateIssued <= filterDto.MaxDateIssued.Value);

            if (filterDto.MinTotalAmount.HasValue)
                query = query.Where(b => b.TotalAmount >= filterDto.MinTotalAmount.Value);

            if (filterDto.MaxTotalAmount.HasValue)
                query = query.Where(b => b.TotalAmount <= filterDto.MaxTotalAmount.Value);

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
