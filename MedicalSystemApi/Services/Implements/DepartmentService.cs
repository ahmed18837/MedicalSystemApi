using AutoMapper;
using MedicalSystemApi.Models.DTOs.Department;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class DepartmentService(IDepartmentRepository departmentRepository, IDoctorRepository doctorRepository, IMapper mapper) : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly IDoctorRepository _doctorRepository = doctorRepository;
        private readonly IMapper _mapper = mapper;

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
               throw new KeyNotFoundException("Department not found!");
            await _departmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DepartmentDto>> GetFilteredDepartmentsAsync(DepartmentFilterDto filterDto)
        {
            var departments = await _departmentRepository.GetFilteredDepartmentsAsync(filterDto);

            if (departments == null && !departments!.Any())
                throw new Exception("No departments found matching the given criteria.");

            var departmentsDto = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            return departmentsDto;
        }

    }
}


