using MedicalSystemApi.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RequestRegisterDto request);
        Task<string> LoginAsync(RequestLoginDto request);
        Task<string> AssignRoleAsync(string email, string roleName);
        Task<string> ForgetPasswordAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordDto model);
    }
}
