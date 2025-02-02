using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IBillMedicalTestRepository : IGenericRepository<BillMedicalTest>
    {
        Task<IEnumerable<BillMedicalTest>> GetAllWithMedicalTestName();
        Task<BillMedicalTest> GetOneWithMedicalTestName(int id);
        Task<bool> BillExistsAsync(int billId);
        Task<bool> MedicalTestExistsAsync(int medicalTestId);
    }
}
