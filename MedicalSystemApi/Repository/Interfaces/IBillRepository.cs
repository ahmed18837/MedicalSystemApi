using MedicalSystemApi.Models.DTOs.Bill;
using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        Task<bool> PatientExistsAsync(int patientId);
        Task<IEnumerable<Bill>> GetBillsByPatientIdAsync(int patientId);
        Task<bool> UpdateTotalAmountAsync(int billId, decimal amount);
        Task<IEnumerable<Bill>> GetFilteredBillsAsync(BillFilterDto filterDto);
    }
}
