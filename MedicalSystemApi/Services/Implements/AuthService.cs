using AutoMapper;
using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.Auth;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, ITokenService tokenService
            , IEmailService emailService, IMapper mapper)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<string> RegisterAsync(RequestRegisterDto request)
        {
            var existingUser = await _authRepository.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return "A user with this email already exists";

            if (!await _authRepository.IsEmailValid(request.Email))
            {
                throw new InvalidOperationException("Invalid Email Address!");
            }

            if (!await _authRepository.IsPhoneNumberValid(request.PhoneNumber))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }

            var identityUser = _mapper.Map<ApplicationUser>(request);

            var identityResult = await _authRepository.CreateUserAsync(identityUser, request.Password);
            if (!identityResult.Succeeded)
                return string.Join(", ", identityResult.Errors.Select(e => e.Description));

            if (!await _authRepository.RoleExistsAsync("User"))
                await _authRepository.AddToRoleAsync(identityUser, "User");

            identityResult = await _authRepository.AddToRoleAsync(identityUser, "User");
            if (!identityResult.Succeeded)
                return string.Join(", ", identityResult.Errors.Select(e => e.Description));

            return "User was registered ... Please login.";
        }

        public async Task LoginAsync(RequestLoginDto request)
        {
            var user = await _authRepository.FindByEmailAsync(request.Email) ??
                 throw new Exception("UserName or Password is incorrect");

            var isValidPassword = await _authRepository.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword)
                throw new Exception("UserName or Password is incorrect");

            var roles = await _authRepository.GetUserRolesAsync(user);
            if (roles == null || roles.Count == 0)
                throw new Exception("User has no assigned roles");

            // إنشاء التوكن
            var jwtToken = _tokenService.CreateJWTToken(user, roles.ToArray());

            // إرسال التوكن عبر البريد الإلكتروني
            var message = new Message(new[] { request.Email }, "Sent Token", jwtToken);
            _emailService.SendEmail(message);
        }
    }
}
