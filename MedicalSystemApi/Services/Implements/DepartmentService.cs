using AutoMapper;
using MedicalSystemApi.Models.DTOs.Department;
using MedicalSystemApi.Models.DTOs.Doctor;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IDoctorRepository doctorRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departmentList = await _departmentRepository.GetAllAsync() ??
               throw new Exception("There are not Departments!");

            var departmentListDto = _mapper.Map<IEnumerable<DepartmentDto>>(departmentList);
            return departmentListDto;
        }

        public async Task<DepartmentDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var department = await _departmentRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Department not fount");

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return departmentDto;
        }

        public async Task AddAsync(CreateDepartmentDto createDepartmentDto)
        {
            if (createDepartmentDto == null) throw new ArgumentNullException("Input data cannot be null");
            if (string.IsNullOrWhiteSpace(createDepartmentDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }
            var department = _mapper.Map<Department>(createDepartmentDto);
            await _departmentRepository.AddAsync(department);
        }

        public async Task UpdateAsync(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            if (updateDepartmentDto == null) throw new ArgumentNullException("Input data cannot be null");

            var department = await _departmentRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Department not found");

            var updatedDepartment = _mapper.Map(updateDepartmentDto, department);
            await _departmentRepository.UpdateAsync(id, updatedDepartment);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id) ??
               throw new KeyNotFoundException("Department not found");
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            if (departmentId <= 0)
            {
                throw new ArgumentException("Invalid Department ID.");
            }

            var doctors = await _departmentRepository.GetDoctorsByDepartmentIdAsync(departmentId);

            if (!doctors.Any())
            {
                throw new KeyNotFoundException($"No doctors found for Department ID {departmentId}");
            }

            var doctorsDto = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            return doctorsDto;
        }

        public async Task RemoveDoctorFromDepartmentAsync(int departmentId, int doctorId)
        {
            if (departmentId <= 0 || doctorId <= 0)
                throw new ArgumentException("Invalid Department ID or Doctor ID.");

            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null || doctor.DepartmentId != departmentId)
                throw new KeyNotFoundException($"Doctor with ID {doctorId} is not part of Department {departmentId}");

            bool success = await _departmentRepository.RemoveDoctorFromDepartmentAsync(departmentId, doctorId);
            if (!success)
                throw new Exception("Failed to remove doctor from department");
        }
    }
}
