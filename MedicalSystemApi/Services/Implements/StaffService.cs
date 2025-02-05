using AutoMapper;
using MedicalSystemApi.Models.DTOs.Staff;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IMapper _mapper;

        public StaffService(IStaffRepository staffRepository, IMapper mapper)
        {
            _staffRepository = staffRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StaffDto>> GetAllAsync()
        {
            var staffList = await _staffRepository.GetAllWithDepartmentNameAsync() ??
                throw new Exception("There are not Staffs!");

            var staffListDto = _mapper.Map<IEnumerable<StaffDto>>(staffList); // ابطىء نسبيا ولكن سهل فى التعديل
            return staffListDto;
        }

        public async Task<StaffDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var staff = await _staffRepository.GetStaffWithDepartmentAsync(id) ??
                throw new InvalidOperationException("Staff not fount");

            var staffDto = _mapper.Map<StaffDto>(staff);
            return staffDto;
        }

        public async Task AddAsync(CreateStaffDto createStaffDto)
        {
            if (createStaffDto == null) throw new ArgumentNullException("Input data cannot be null");

            // التحقق من صحة البيانات
            var staffUpdated = _mapper.Map<UpdateStaffDto>(createStaffDto);

            await ValidateStaffData(staffUpdated);

            var staff = _mapper.Map<Staff>(createStaffDto);

            staff.HireDate = DateTime.Now;

            await _staffRepository.AddAsync(staff);
        }

        public async Task UpdateAsync(int id, UpdateStaffDto updateStaffDto)
        {
            if (updateStaffDto == null) throw new ArgumentNullException("Input data cannot be null");

            var staff = await _staffRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Staff not found");

            await ValidateStaffData(updateStaffDto);

            var staffUpdated = _mapper.Map(updateStaffDto, staff);

            staff.HireDate = DateTime.Now;

            await _staffRepository.UpdateAsync(id, staffUpdated);
        }

        public async Task DeleteAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id) ??
               throw new KeyNotFoundException("Staff not found");

            await _staffRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<StaffDto>> GetStaffByDepartmentAsync(int departmentId)
        {
            if (departmentId <= 0)
                throw new ArgumentException("Department ID must be a positive number.");

            if (!await _staffRepository.DepartmentExistsAsync(departmentId))
            {
                throw new KeyNotFoundException("Department not found!");
            }
            var staffList = await _staffRepository.GetStaffByDepartmentAsync(departmentId);
            var staffListDto = _mapper.Map<IEnumerable<StaffDto>>(staffList);
            return staffListDto;
        }

        public async Task<Dictionary<string, int>> GetStaffCountByRoleAsync()
        {
            var data = await _staffRepository.GetStaffCountByRoleAsync();
            if (data == null || !data.Any())
                throw new InvalidOperationException("No staff data found for roles");

            return data;
        }

        public async Task<Dictionary<string, int>> GetStaffCountByDepartmentAsync()
        {
            var data = await _staffRepository.GetStaffCountByDepartmentAsync();
            if (data == null || !data.Any())
                throw new InvalidOperationException("No staff data found for departments.");

            return data;
        }

        public async Task<int> GetYearsOfServiceAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid staff ID.");

            var years = await _staffRepository.GetYearsOfServiceAsync(id);
            if (years == -1)
                throw new KeyNotFoundException("Staff member not found.");

            return years;
        }

        public async Task<StaffDto> SearchWithEmailStaffAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email number is required");

            if (!await _staffRepository.IsEmailValid(email))
            {
                throw new InvalidOperationException("Invalid Email Address!");
            }

            var staff = await _staffRepository.SearchWithEmailStaffAsync(email) ??
                throw new KeyNotFoundException("Staff member not found");
            var staffDto = _mapper.Map<StaffDto>(staff);
            return staffDto;
        }

        public async Task<StaffDto> SearchWithPhoneStaffAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone number is required");

            if (!await _staffRepository.IsPhoneNumberValid(phone))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }

            var staff = await _staffRepository.SearchWithPhoneStaffAsync(phone) ??
                throw new KeyNotFoundException("Staff member not found");
            var staffDto = _mapper.Map<StaffDto>(staff);
            return staffDto;
        }

        private async Task ValidateStaffData(UpdateStaffDto staffDto)
        {
            if (string.IsNullOrEmpty(staffDto.FullName))
            {
                throw new ArgumentException("Full name is required");
            }
            if (string.IsNullOrEmpty(staffDto.RoleStaff))
            {
                throw new ArgumentException("Role name is required");
            }

            if (!await _staffRepository.IsEmailValid(staffDto.Email))
            {
                throw new InvalidOperationException("Invalid Email Address!");
            }

            if (await _staffRepository.EmailExistsAsync(staffDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            if (!await _staffRepository.DepartmentExistsAsync(staffDto.DepartmentId))
            {
                throw new KeyNotFoundException("Department not found!");
            }

            if (await _staffRepository.PhoneExistsAsync(staffDto.Phone))
            {
                throw new InvalidOperationException("Phone Number already exists");
            }

            if (!await _staffRepository.IsPhoneNumberValid(staffDto.Phone))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }
        }


        public async Task UpdateStaffRoleOrDepartmentAsync(UpdateStaffRoleOrDepartmentDto updateDto)
        {
            if (updateDto == null)
            {
                throw new ArgumentNullException("Input data cannot be null");
            }
            if (updateDto.StaffId <= 0)
            {
                throw new ArgumentException("Invalid Staff ID");
            }
            if (!await _staffRepository.StaffIdExistsAsync(updateDto.StaffId))
            {
                throw new KeyNotFoundException("Staff member not found");
            }
            if (!string.IsNullOrWhiteSpace(updateDto.RoleStaff) && !IsValidRole(updateDto.RoleStaff))
            {
                throw new ArgumentException("Invalid role staff");
            }
            if (!await _staffRepository.DepartmentExistsAsync(updateDto.DepartmentId))
            {
                throw new KeyNotFoundException("Department not found!");
            }
            await _staffRepository.UpdateStaffRoleOrDepartmentAsync(updateDto.StaffId, updateDto.RoleStaff, updateDto.DepartmentId);
        }

        private bool IsValidRole(string role)
        {
            // Define a list of valid roles
            var validRoles = new List<string>
    {
        "Secretary",
        "Technician",
        "Receptionist",
        "Manager",
        "Nurse",
        "Doctor"
    };
            return validRoles.Contains(role);
        }
    }
}
