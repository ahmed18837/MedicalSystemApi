using AutoMapper;
using MedicalSystemApi.Models.DTOs.Staff;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class StaffService(IStaffRepository staffRepository, IFileService fileService, IMapper mapper) : IStaffService
    {
        private readonly IStaffRepository _staffRepository = staffRepository;
        private readonly IFileService _fileService = fileService;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<StaffDto>> GetAllWithDepartmentNameAsync()
        {
            var staffListDto = await _staffRepository.GetAllWithDepartmentNameAsync() ??
                throw new Exception("There are not Staffs!");

            return staffListDto;
        }

        public async Task<StaffDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var staffDto = await _staffRepository.GetStaffWithDepartmentAsync(id) ??
                throw new InvalidOperationException("Staff not fount");

            return staffDto;
        }

        public async Task AddAsync(CreateStaffDto createStaffDto)
        {
            if (createStaffDto == null) throw new ArgumentNullException("Input data cannot be null");

            // التحقق من صحة البيانات
            var staffUpdated = _mapper.Map<UpdateStaffDto>(createStaffDto);

            await ValidateStaffData(staffUpdated);

            if (await _staffRepository.EmailExistsAsync(createStaffDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            if (await _staffRepository.PhoneExistsAsync(createStaffDto.Phone))
            {
                throw new InvalidOperationException("Phone Number already exists");
            }
            var staff = _mapper.Map<Staff>(createStaffDto);

            staff.ImagePath = createStaffDto.Image != null
            ? _fileService.SaveFile(createStaffDto.Image, "Staffs") : null;

            staff.HireDate = DateTime.Now;

            await _staffRepository.AddAsync(staff);
        }

        public async Task UpdateAsync(int id, UpdateStaffDto updateStaffDto)
        {
            if (updateStaffDto == null) throw new ArgumentNullException("Input data cannot be null");

            var staff = await _staffRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Staff not found");

            await ValidateStaffData(updateStaffDto);
            _mapper.Map(updateStaffDto, staff);

            if (updateStaffDto.Image != null)
            {
                if (!string.IsNullOrEmpty(staff.ImagePath))
                {
                    await _fileService.DeleteFileAsync(staff.ImagePath!);
                }
            }

            staff.ImagePath = updateStaffDto.Image != null
           ? _fileService.SaveFile(updateStaffDto.Image, "Staffs") : null;

            staff.HireDate = DateTime.Now;

            await _staffRepository.UpdateAsync(id, staff);
        }

        public async Task DeleteAsync(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id) ??
               throw new KeyNotFoundException("Staff not found");

            if (!string.IsNullOrEmpty(staff.ImagePath))
            {
                await _fileService.DeleteFileAsync(staff.ImagePath);
            }
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
            var staffList = await _staffRepository.GetStaffWithDepartmentIdAsync(departmentId);
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

        public async Task UpdateStaffImageAsync(string staffId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new KeyNotFoundException("No file uploaded");
            }

            var staff = await _staffRepository.GetByIdAsync(int.Parse(staffId)) ??
                throw new KeyNotFoundException("Staff not found");

            if (!string.IsNullOrEmpty(staff.ImagePath))
            {
                await _fileService.DeleteFileAsync(staff.ImagePath);
            }

            var newImagePath = _fileService.SaveFile(file, "Staffs");

            // تحديث مسار الصورة في قاعدة البيانات
            staff.ImagePath = newImagePath!;
            await _staffRepository.UpdateAsync(int.Parse(staffId), staff);
        }

        public async Task<IEnumerable<StaffDto>> GetFilteredStaffAsync(StaffFilterDto filterDto)
        {
            var staffList = await _staffRepository.GetFilteredStaffAsync(filterDto);
            if (staffList == null && !staffList!.Any())
                throw new Exception("No staff members found matching the given criteria");

            var staffListDto = _mapper.Map<IEnumerable<StaffDto>>(staffList);
            return staffListDto;
        }



        private async Task ValidateStaffData(UpdateStaffDto staffDto)
        {
            if (!await _staffRepository.IsEmailValid(staffDto.Email))
            {
                throw new InvalidOperationException("Invalid Email Address!");
            }

            if (!await _staffRepository.DepartmentExistsAsync(staffDto.DepartmentId))
            {
                throw new KeyNotFoundException("Department not found!");
            }

            if (!await _staffRepository.IsPhoneNumberValid(staffDto.Phone))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }
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
