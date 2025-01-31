using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MedicalSystemApi.Repository.Implement
{
    public class StaffRepository : GenericRepository<Staff>, IStaffRepository
    {
        private readonly AppDbContext _dbContext;
        public StaffRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Staff>> GetAllWithDepartmentAsync()
        {
            return await _dbContext.Staffs
             .AsNoTracking()
            .Include(s => s.Department)
            .ToListAsync();
        }

        public async Task<Staff> GetStaffWithDepartmentAsync(int id)
        {
            return await _dbContext.Staffs
            .AsNoTracking()
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.Id == id);
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
    }
}
