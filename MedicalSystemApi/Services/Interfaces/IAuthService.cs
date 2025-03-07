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
        Task<string> Send2FACodeAsync(string email);
        Task<string> Verify2FACodeAsync(Verify2FACodeDto model);
        public Task<string> UnlockUserAsync(string email);
        Task<string> ChangePasswordAsync(ChangePasswordDto model);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(string Id);
        Task UpdateUserAsync(string Id, UpdateUserDto model);

        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<IEnumerable<string>> GetRolesByEmailAsync(string email);
        Task DeleteRoleAsync(string roleName);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName);

        public Task UpdateRoleAsync(UpdateRoleDto model);
        Task<string> Resend2FACodeAsync(string email);

        Task AddRoleAsync(string roleName);
        Task<string> RemoveUserFromRoleAsync(string email, string role);
        Task<string> AddUserAsync(AddUserDto userDto, string password, string role);
        Task<string> DeleteUserAsync(string email);
    }
}
