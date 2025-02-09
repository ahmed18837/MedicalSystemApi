using Microsoft.AspNetCore.Identity;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateJWTToken(IdentityUser user, string[] roles);
        Task<string> AssignRoleAsync(string id, string role);
    }
}
