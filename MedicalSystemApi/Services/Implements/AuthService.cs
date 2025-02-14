using AutoMapper;
using MedicalSystemApi.Helpers;
using MedicalSystemApi.Models.DTOs.Auth;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<string> ForgetPasswordAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            var resetCode = GenerateCode();


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

        public async Task<string> Send2FACodeAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            // التحقق مما إذا كان هناك كود موجود ولكنه منتهي الصلاحية
            if (user.TwoFactorCodeExpiration != null && user.TwoFactorCodeExpiration > DateTime.UtcNow)
                return "A valid 2FA code has already been sent. Please check your email.";

            var twoFactorCode = GenerateCode();

            user.TwoFactorCode = twoFactorCode;
            user.TwoFactorCodeExpiration = DateTime.UtcNow.AddMinutes(10);

            await _manager.UpdateAsync(user);

            var subject = "Your 2FA Code";
            var message = $"Your two-factor authentication code is: {twoFactorCode}";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return "A 2FA code has been sent to your email.";
        }

        public async Task<string> Resend2FACodeAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            if (user.TwoFactorSentAt != null && (DateTime.UtcNow - user.TwoFactorSentAt.Value).TotalSeconds < 60)
            {
                throw new Exception("Please wait at least 1 minute before requesting a new code.");
            }

            // التحقق مما إذا كان الحساب مقفلًا حاليًا
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                throw new Exception($"Your account is locked. Try again at {user.LockoutEnd.Value.ToLocalTime()}.");
            }

            // إعادة ضبط المحاولات إذا مرت ساعة
            if (user.LastTwoFactorAttempt != null && (DateTime.UtcNow - user.LastTwoFactorAttempt.Value).TotalHours >= 1)
            {
                user.TwoFactorAttempts = 0;
            }

            if (user.TwoFactorAttempts >= 5)
            {
                throw new Exception("You have exceeded the maximum number of attempts. Please try again later.");
            }

            var newCode = GenerateCode();
            user.TwoFactorCode = newCode;
            user.TwoFactorCodeExpiration = DateTime.UtcNow.AddMinutes(10);

            user.TwoFactorSentAt = DateTime.UtcNow;

            user.TwoFactorAttempts += 1;
            user.LastTwoFactorAttempt = DateTime.UtcNow;

            await _manager.UpdateAsync(user);

            await _emailService.SendEmailAsync(user.Email, "Your 2FA Code", $"Your new 2FA code is: {newCode}");

            return "A new 2FA code has been sent to your email.";
        }

        public async Task<string> Verify2FACodeAsync(Verify2FACodeDto model)
        {
            var user = await _manager.FindByEmailAsync(model.Email)
             ?? throw new Exception("User not found");

            // التحقق مما إذا كان الكود قد انتهت صلاحيته
            if (user.TwoFactorCode == null || user.TwoFactorCodeExpiration < DateTime.UtcNow)
                throw new Exception("The 2FA code has expired. Please request a new one.");

            // التحقق من صحة الكود
            if (user.TwoFactorCode != model.Code)
            {
                user.FailedTwoFactorAttempts++;

                // إذا تجاوز 5 محاولات خاطئة، يتم قفل الحساب
                if (user.FailedTwoFactorAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(2); //  قفل الحساب لمدة 15 دقيقة
                    await _manager.UpdateAsync(user);
                    throw new Exception("Too many failed attempts. Your account is locked for 15 minutes.");
                }

                await _manager.UpdateAsync(user);
                throw new Exception("Invalid 2FA code.");
            }

            // Reset the 2FA code after successful verification
            user.FailedTwoFactorAttempts = 0;
            user.TwoFactorAttempts = 0;
            user.LockoutEnd = null;
            user.TwoFactorCode = null;
            user.TwoFactorCodeExpiration = null;

            await _manager.UpdateAsync(user);

            return "2FA verification successful";
        }

        public async Task<string> UnlockUserAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            //  التحقق مما إذا كان الحساب مقفلًا حاليًا
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                user.LockoutEnd = null; // إزالة القفل
                user.FailedTwoFactorAttempts = 0; // إعادة تعيين المحاولات الفاشلة
                await _manager.UpdateAsync(user);
                return "User account has been unlocked successfully.";
            }

            return "User account is not locked.";
        }


        public async Task<string> ChangePasswordAsync(ChangePasswordDto model)
        {
            var user = await _manager.FindByEmailAsync(model.Email)
             ?? throw new Exception("User not found");

            // Verify old password
            var isOldPasswordCorrect = await _manager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isOldPasswordCorrect)
                throw new Exception("Incorrect Current password");

            // Ensure new password is not the same as the old one
            if (model.CurrentPassword == model.NewPassword)
                throw new Exception("New password cannot be the same as the Current password");

            var result = await _manager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password change failed: {errors}");
            }

            return "Password changed successfully";
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = _manager.Users.ToList();

            var usersDto = _mapper.Map<List<UserDto>>(users);

            // User لكل  Role لتحديد 
            foreach (var userDto in usersDto)
            {
                var user = users.FirstOrDefault(u => u.Id == userDto.Id);
                userDto.Roles = await _manager.GetRolesAsync(user);
            }

            return usersDto;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _manager.FindByEmailAsync(email)
                 ?? throw new Exception("User not found");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _manager.GetRolesAsync(user); // تعيين الأدوار يدويًا

            return userDto;
        }

        public async Task<UserDto> GetUserByIdAsync(string Id)
        {
            var user = await _manager.FindByIdAsync(Id)
                 ?? throw new Exception("User not found");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _manager.GetRolesAsync(user);

            return userDto;
        }

        public async Task UpdateUserAsync(string Id, UpdateUserDto model)
        {
            var user = await _manager.FindByIdAsync(Id)
                ?? throw new Exception("User not found");

            if (user.FullName == model.FullName && user.PhoneNumber == model.PhoneNumber
                && user.Email == model.Email)
                throw new Exception("No changes detected. The data is already up to date.");

            _mapper.Map(model, user);

            var result = await _manager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update user: {errors}");
            }
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync() ??
                throw new Exception("No Roles Exist!");

            return roles;
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName) ??
                throw new Exception("Role not found");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to delete role: {errors}");
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                throw new Exception("Role does not exist");

            var users = await _manager.GetUsersInRoleAsync(roleName) ??
                throw new Exception("Users does not exist");

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            // User لكل  Role لتحديد 
            foreach (var userDto in usersDto)
            {
                var user = users.FirstOrDefault(u => u.Id == userDto.Id);
                userDto.Roles = await _manager.GetRolesAsync(user);
            }

            return usersDto;
        }

        public async Task UpdateRoleAsync(UpdateRoleDto model)
        {
            var role = await _roleManager.FindByNameAsync(model.OldRoleName) ??
                throw new Exception("Role not found");

            var roleExists = await _roleManager.RoleExistsAsync(model.NewRoleName);
            if (roleExists)
                throw new Exception("New role name already exists");

            role.Name = model.NewRoleName;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update role: {errors}");
            }
        }

        public async Task AddRoleAsync(string roleName)
        {
            // التحقق مما إذا كان الدور موجودًا بالفعل
            if (await _roleManager.RoleExistsAsync(roleName))
                throw new Exception("Role already exists.");

            var role = new IdentityRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString() // تأكد من تعيين `ConcurrencyStamp`
            };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create role: {errors}");
            }
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

        private string GenerateCode()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString(); // 🔹 رمز مكون من 8 أرقام
        }

    }
}
