using MedicalSystemApi.Models.DTOs.Auth;

namespace MedicalSystemApi.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
        public string GenerateResetToken(string email);
    }
}
