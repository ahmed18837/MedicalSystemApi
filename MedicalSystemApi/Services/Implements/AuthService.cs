using AutoMapper;
using MedicalSystemApi.Helpers;
using MedicalSystemApi.Models.DTOs.Auth;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalSystemApi.Services.Implements
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _manager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        private readonly IAuthRepository _authRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, UserManager<ApplicationUser> manager, RoleManager<IdentityRole> roleManager
            , IMapper mapper, IOptions<JWT> jwt, IEmailService emailService)
        {
            _authRepository = authRepository;
            _manager = manager;
            _roleManager = roleManager;
            _emailService = emailService;
            _mapper = mapper;
            _jwt = jwt.Value;
        }

        public async Task<IdentityResult> RegisterAsync(RequestRegisterDto request)
        {
            var existingUser = await _manager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("A user with this email already exists");

            if (!await _authRepository.IsEmailValid(request.Email))
                throw new InvalidOperationException("Invalid Email Address!");

            if (!await _authRepository.IsPhoneNumberValid(request.PhoneNumber))
                throw new InvalidOperationException("Invalid Phone Number!");

            if (await _authRepository.PhoneExistsAsync(request.PhoneNumber))
                throw new InvalidOperationException("Phone Number  already exists!");

            var appUser = _mapper.Map<ApplicationUser>(request);

            // ✅ إنشاء المستخدم
            var identityResult = await _manager.CreateAsync(appUser, request.Password);
            if (!identityResult.Succeeded)
            {
                //throw new Exception("InValid Email Or Password!");
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }


            // ✅ إضافة المستخدم إلى الدور "User"
            identityResult = await _manager.AddToRoleAsync(appUser, "User");
            if (!identityResult.Succeeded)
            {
                //throw new Exception("Failed to assign the role 'User'");
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign the role 'User': {errors}");
            }

            return identityResult;
        }

        public async Task<string> LoginAsync(RequestLoginDto request)
        {
            var user = await _manager.FindByEmailAsync(request.Email) ??
                 throw new Exception("UserName or Password is incorrect");

            var isValidPassword = await _manager.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword)
                throw new Exception("UserName or Password is incorrect");

            var roles = await _manager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
                throw new Exception("User has no assigned roles");

            var appUser = _mapper.Map<ApplicationUser>(request);

            // Crate Token
            var jwtToken = await CreateJwtToken(appUser);

            string subject = "Your Login Token";
            string body = $"<h2>Your JWT Token</h2><p>{jwtToken}</p><p>Use this token for authentication.</p>";


            bool emailSent = await _emailService.SendEmailAsync(user.Email, subject, body);
            if (!emailSent)
                return "Login successful, but failed to send token via email";

            return "Login successful. Token sent to your email";
        }

        public async Task<string> AssignRoleAsync(string email, string roleName)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new Exception("Role does not exist");

            var result = await _manager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign role: {errors}");
            }

            return $"Role '{roleName}' assigned to user '{email}' successfully";
        }

        private async Task<string> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _manager.GetClaimsAsync(user);
            var roles = await _manager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public async Task<string> ForgetPasswordAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            var resetCode = GenerateResetCode();


            user.ResetCode = resetCode;
            await _manager.UpdateAsync(user);

            var subject = "Password Reset Code";
            var message = $"Your password reset code is: {resetCode}";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return "A password reset code has been sent to your email.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _manager.FindByEmailAsync(model.Email) ??
               throw new Exception("User not found");

            //  التحقق من الرمز
            if (user.ResetCode != model.ResetCode)
                throw new Exception("Invalid reset code");

            // التحقق مما إذا كانت كلمة المرور الجديدة هي نفس القديمة
            var passwordCheck = await _manager.CheckPasswordAsync(user, model.NewPassword);
            if (passwordCheck)
                throw new Exception("New password cannot be the same as the current password.");


            //  إعادة تعيين كلمة المرور
            var resetToken = await _manager.GeneratePasswordResetTokenAsync(user);

            var result = await _manager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to reset password: {errors}");
            }

            //  إزالة الرمز بعد الاستخدام
            user.ResetCode = null;
            await _manager.UpdateAsync(user);

            return "Password has been reset successfully!";
        }












        private string GenerateResetCode()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString(); // 🔹 رمز مكون من 8 أرقام
        }
    }
}
