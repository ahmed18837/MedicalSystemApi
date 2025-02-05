using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly AppDbContext _dbContext;
        public DepartmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            return await _dbContext.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .Where(d => d.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<bool> RemoveDoctorFromDepartmentAsync(int departmentId, int doctorId)
        {
            var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId && d.DepartmentId == departmentId);
            if (doctor == null) return false;

            doctor.DepartmentId = null;
            _dbContext.Doctors.Update(doctor);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
