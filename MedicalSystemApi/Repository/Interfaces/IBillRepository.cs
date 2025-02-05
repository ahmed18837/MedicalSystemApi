using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        Task<bool> PatientExistsAsync(int patientId);
        Task<Bill> GetBillsByPatientIdAsync(int patientId);
        Task<bool> UpdateTotalAmountAsync(int billId, decimal amount);
    }
}
