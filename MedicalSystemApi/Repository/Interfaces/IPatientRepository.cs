using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<bool> IsPhoneNumberValid(string phoneNumber);
    }
}
