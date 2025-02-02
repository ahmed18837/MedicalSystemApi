using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        private readonly AppDbContext _dbContext;
        public BillRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _dbContext.Patients
                 .AnyAsync(i => i.Id == patientId);
        }
    }
}
