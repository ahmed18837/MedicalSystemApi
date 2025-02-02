using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;

namespace MedicalSystemApi.Repository.Implement
{
    public class MedicalTestRepository : GenericRepository<MedicalTest>, IMedicalTestRepository
    {
        public MedicalTestRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
