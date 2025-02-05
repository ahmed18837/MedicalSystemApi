using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<IEnumerable<Doctor>> GetDoctorsByDepartmentIdAsync(int departmentId);
        Task<bool> RemoveDoctorFromDepartmentAsync(int departmentId, int doctorId);
    }
}
