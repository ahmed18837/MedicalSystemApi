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
            var staffList = await _staffRepository.GetAllWithDepartmentAsync() ??
                throw new Exception("There are not Staffs!");

            var staffListDto = _mapper.Map<IEnumerable<StaffDto>>(staffList); // ابطىء نسبيا ولكن سهل فى التعديل
            return staffListDto;

            //return staffList.Select(s => new StaffDto // اسرع ولكن صعب فى التعديل
            //{
            //    Id = s.Id,
            //    FullName = s.FullName,
            //    RoleStaff = s.RoleStaff,
            //    Phone = s.Phone,
            //    Email = s.Email,
            //    HireDate = s.HireDate,
            //    DepartmentName = s.Department?.Name ?? "Unknown" // جلب اسم القسم فقط
            //}).ToList();
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

    }
}
