using MedicalSystemApi.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace MedicalSystemApi.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> IsEmailValid(string email);
        Task<bool> IsPhoneNumberValid(string phoneNumber);
        Task<bool> PhoneExistsAsync(string phoneNumber);
        Task CreateRoleAsync(string roleName);
    }
}
