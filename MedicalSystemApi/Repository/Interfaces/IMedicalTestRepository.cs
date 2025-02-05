using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IMedicalTestRepository : IGenericRepository<MedicalTest>
    {
        Task<IEnumerable<MedicalTest>> GetExpensiveTests(decimal minCost);
        Task<IEnumerable<MedicalTest>> SearchMedicalTests(string searchTerm);
        Task AssignMedicalTestToBill(int testId, int billId);
        Task<bool> UpdateMedicalTestCost(int testId, decimal newCost);
    }
}
