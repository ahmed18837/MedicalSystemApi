using MedicalSystemApi.Models.DTOs.Auth;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RequestRegisterDto request);
        Task LoginAsync(RequestLoginDto request);
    }
}
