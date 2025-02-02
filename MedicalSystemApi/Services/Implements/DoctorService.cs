using AutoMapper;
using MedicalSystemApi.Models.DTOs.Doctor;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctorList = await _doctorRepository.GetAllWithDepartmentName() ??
                throw new Exception("There are not Doctors!");

            return doctorList.Select(d => new DoctorDto
            {
                Id = d.Id,
                FullName = d.FullName,
                Age = d.Age,
                Gender = d.Gender,
                Phone = d.Phone,
                Email = d.Email,
                Specialty = d.Specialty,
                WorkingHours = d.WorkingHours,
                DepartmentName = d.Department.Name
            }); // اسرع

            //var doctorListDto = _mapper.Map<IEnumerable<DoctorDto>>(doctorList);
            //return doctorListDto;
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var doctor = await _doctorRepository.GetDoctorWithDepartmentName(id) ??
                throw new InvalidOperationException("Doctor not fount");

            var doctorDto = _mapper.Map<DoctorDto>(doctor);
            return doctorDto;
        }


        public async Task AddAsync(CreateDoctorDto createDoctorDto)
        {
            if (createDoctorDto == null) throw new ArgumentNullException("Input data cannot be null");

            await ValidateDoctorData(createDoctorDto);

            var doctor = _mapper.Map<Doctor>(createDoctorDto);
            await _doctorRepository.AddAsync(doctor);
        }

        public async Task UpdateAsync(int id, UpdateDoctorDto updateDoctorDto)
        {
            if (updateDoctorDto == null) throw new ArgumentNullException("Input data cannot be null");

            var doctor = await _doctorRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Doctor not found");

            var createdDoctorDto = _mapper.Map<CreateDoctorDto>(updateDoctorDto);

            await ValidateDoctorData(createdDoctorDto);

            var doctorUpdated = _mapper.Map(updateDoctorDto, doctor);

            await _doctorRepository.UpdateAsync(id, doctorUpdated);
        }

        public async Task DeleteAsync(int id)
        {
            var staff = await _doctorRepository.GetByIdAsync(id) ??
              throw new KeyNotFoundException("Doctor not found");

            await _doctorRepository.DeleteAsync(id);
        }

        private async Task ValidateDoctorData(CreateDoctorDto doctorDto)
        {
            if (string.IsNullOrEmpty(doctorDto.FullName))
            {
                throw new ArgumentException("Full name is required");
            }

            if (!await _doctorRepository.IsEmailValid(doctorDto.Email))
            {
                throw new InvalidOperationException("Invalid Email Address!");
            }

            if (await _doctorRepository.EmailExistsAsync(doctorDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            if (!await _doctorRepository.DepartmentExistsAsync(doctorDto.DepartmentId))
            {
                throw new KeyNotFoundException("Department not found!");
            }

            if (await _doctorRepository.PhoneExistsAsync(doctorDto.Phone))
            {
                throw new InvalidOperationException("Phone Number already exists");
            }

            if (!await _doctorRepository.IsPhoneNumberValid(doctorDto.Phone))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }
        }
    }
}
