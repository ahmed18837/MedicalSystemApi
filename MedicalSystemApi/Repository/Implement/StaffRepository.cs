using MedicalSystemApi.Data;
using MedicalSystemApi.Models.DTOs.Staff;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MedicalSystemApi.Repository.Implement
{
    public class StaffRepository(AppDbContext dbContext) : GenericRepository<Staff>(dbContext), IStaffRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<StaffDto>> GetAllWithDepartmentNameAsync()
        {
            return await _dbContext.Staffs
             .AsNoTrackingWithIdentityResolution()
            .Select(s => new StaffDto
            {
                FullName = s.FullName,
                RoleStaff = s.RoleStaff,
                Phone = s.Phone,
                Email = s.Email,
                HireDate = s.HireDate,
                DepartmentName = s.Department.Name, // تضمين اسم القسم
                ImagePath = s.ImagePath ?? string.Empty
            })
            .ToListAsync();
        }

        public async Task<StaffDto> GetStaffWithDepartmentAsync(int id)
        {
            return await _dbContext.Staffs
                .Where(s => s.Id == id)
             .AsNoTrackingWithIdentityResolution()
            .Select(s => new StaffDto
            {
                FullName = s.FullName,
                RoleStaff = s.RoleStaff,
                Phone = s.Phone,
                Email = s.Email,
                HireDate = s.HireDate,
                DepartmentName = s.Department.Name, // تضمين اسم القسم
                ImagePath = s.ImagePath ?? string.Empty
            })
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<StaffDto>> GetStaffWithDepartmentIdAsync(int departmentId)
        {
            return await _dbContext.Staffs
                .Where(s => s.DepartmentId == departmentId)
             .AsNoTrackingWithIdentityResolution()
            .Select(s => new StaffDto
            {
                FullName = s.FullName,
                RoleStaff = s.RoleStaff,
                Phone = s.Phone,
                Email = s.Email,
                HireDate = s.HireDate,
                DepartmentName = s.Department.Name, // تضمين اسم القسم
                ImagePath = s.ImagePath ?? string.Empty
            })
            .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Staffs
                 .AnyAsync(s => s.Email.ToUpper() == email.ToUpper());
        }

        public async Task<bool> DepartmentExistsAsync(int departmentId)
        {
            return await _dbContext.Departments
                 .AnyAsync(d => d.Id == departmentId);
        }

        public async Task<bool> IsPhoneNumberValid(string phoneNumber)
        {
            // هذا المثال يتحقق من أن الرقم يبدأ بـ "+" يتبعه كود الدولة (من 1 إلى 3 أرقام)، ثم يتبعه رقم الهاتف المحلي
            //var phonePattern = @"^\+(\d{1,3})\d{8,15}$";
            // التحقق من أن الرقم يبدأ بـ +20 ويتبعه رقم هاتف من 10 أرقام
            var egyptPhonePattern = @"^\+20\d{10}$";
            return Regex.IsMatch(phoneNumber, egyptPhonePattern);
        }

        public async Task<bool> IsEmailValid(string email)
        {
            var emailPattern = @"^[\w-\.]+@([\w-]+\.)+[a-zA-Z]{2,7}$"; // @"^ بداية السلسلة
                                                                       // [\w-\.]+    // ( '.' , '-' جزء اسم المستخدم (يتضمن الحروف والأرقام والرموز   
                                                                       // @ علامة @ بين اسم المستخدم والنطاق
                                                                       // ([\w-]+\.)+  جزء النطاق (يحتوي على الحروف والأرقام والرموز '-' و '.' ويتكرر واحدًا أو أكثر)
                                                                       // [a-zA-Z]{2,7} // اللاحقة (TLD) من 2 إلى 7 أحرف
                                                                       // @"^ بداية السلسلة
                                                                       // @"^ بداية السلسلة
                                                                       //$";
            return Regex.IsMatch(email, emailPattern);
        }

        public async Task<bool> PhoneExistsAsync(string phoneNumber)
        {
            return await _dbContext.Staffs
                .AnyAsync(p => p.Phone == phoneNumber);
        }

        public async Task<Dictionary<string, int>> GetStaffCountByRoleAsync()
        {
            var staffCountList = await _dbContext.Staffs
                .AsNoTracking()
                .GroupBy(s => s.RoleStaff)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToListAsync();

            return staffCountList.ToDictionary(x => x.Role, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetStaffCountByDepartmentAsync()
        {
            var staffCountList = await _dbContext.Staffs
                .AsNoTracking()
                .GroupBy(s => s.Department.Name)
                .Select(g => new { DepartmentName = g.Key, Count = g.Count() })
                .ToListAsync();

            return staffCountList.ToDictionary(x => x.DepartmentName, x => x.Count);
        }

        public async Task<int> GetYearsOfServiceAsync(int id)
        {
            var staff = await _dbContext.Staffs.FindAsync(id);
            if (staff == null) return -1;

            return DateTime.Now.Year - staff.HireDate.Year;
        }

        public async Task<Staff> SearchWithPhoneStaffAsync(string phone)
        {
            return await _dbContext.Staffs
               .Include(s => s.Department)
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.Phone == phone);
        }

        public async Task<Staff> SearchWithEmailStaffAsync(string email)
        {
            return await _dbContext.Staffs
               .Include(s => s.Department)
               .AsNoTracking()
               .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<bool> StaffIdExistsAsync(int staffId)
        {
            return await _dbContext.Staffs.AnyAsync(s => s.Id == staffId);
        }

        public async Task<bool> RoleStaffExistsAsync(string roleStaff)
        {
            return await _dbContext.Staffs.AnyAsync(s => s.RoleStaff.ToUpper() == roleStaff.ToUpper());
        }

        public async Task UpdateStaffRoleOrDepartmentAsync(int staffId, string? roleStaff, int? departmentId)
        {
            var staff = await _dbContext.Staffs.FirstOrDefaultAsync(s => s.Id == staffId) ??
                 throw new KeyNotFoundException("Staff member not found");

            if (!string.IsNullOrWhiteSpace(roleStaff))
            {
                staff.RoleStaff = roleStaff;
            }
            if (departmentId.HasValue)
            {
                staff.DepartmentId = departmentId.Value;
            }
            _dbContext.Update(staff);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Staff>> GetFilteredStaffAsync(StaffFilterDto filterDto)
        {
            var query = _dbContext.Staffs
                .Include(s => s.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.FullName))
                query = query.Where(s => s.FullName.Contains(filterDto.FullName));

            if (!string.IsNullOrEmpty(filterDto.RoleStaff))
                query = query.Where(s => s.RoleStaff.ToLower() == filterDto.RoleStaff.ToLower());

            if (filterDto.HireDateFrom.HasValue)
                query = query.Where(s => s.HireDate >= filterDto.HireDateFrom.Value);

            if (filterDto.HireDateTo.HasValue)
                query = query.Where(s => s.HireDate <= filterDto.HireDateTo.Value);

            if (filterDto.DepartmentId.HasValue)
                query = query.Where(s => s.DepartmentId == filterDto.DepartmentId);

            return await query.ToListAsync();
        }
    }
}
