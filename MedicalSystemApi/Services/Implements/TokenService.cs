using MedicalSystemApi.Data;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalSystemApi.Services.Implements
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public string CreateJWTToken(IdentityUser user, string[] roles)
        {
            // Create Claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                   _configuration["Jwt:Issuer"],
                   _configuration["Jwt:Audience"],
                   claims,
                   expires: DateTime.Now.AddMinutes(15),
                   signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> AssignRoleAsync(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id) ??
                throw new Exception($"User not found.");

            // Check if the role exists
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new Exception($"Role '{role}' does not exist.");
            }

            // Check if the user is already in the role
            if (await _userManager.IsInRoleAsync(user, role))
                return $"User '{user.UserName}' is already in the role '{role}'.";

            // Assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                throw new Exception("Failed to assign the role");

            return $"Role '{role}' successfully assigned to user '{user.UserName}'";
        }

    }

}
