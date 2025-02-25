using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class DepartmentRepository(AppDbContext dbContext) : GenericRepository<Department>(dbContext), IDepartmentRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<Department>> GetFilteredDepartmentsAsync(DepartmentFilterDto filterDto)
        {
            var query = _dbContext.Departments.AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.Name))
                query = query.Where(d => EF.Functions.Like(d.Name, $"%{filterDto.Name}%"));

            if (!string.IsNullOrEmpty(filterDto.Location))
                query = query.Where(d => EF.Functions.Like(d.Location, $"%{filterDto.Location}%"));

            return await query.ToListAsync();
        }
    }
}
