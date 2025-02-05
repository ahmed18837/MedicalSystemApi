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
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IFileService fileService, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _fileService = fileService;
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

            if (await _doctorRepository.PhoneExistsAsync(createDoctorDto.Phone))
            {
                throw new InvalidOperationException("Phone Number already exists");
            }
            if (await _doctorRepository.EmailExistsAsync(createDoctorDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var doctor = _mapper.Map<Doctor>(createDoctorDto);

            doctor.ImagePath = createDoctorDto.Image != null
           ? _fileService.SaveFile(createDoctorDto.Image, "Doctors") : null;

            await _doctorRepository.AddAsync(doctor);
        }

        public async Task UpdateAsync(int id, UpdateDoctorDto updateDoctorDto)
        {
            if (updateDoctorDto == null) throw new ArgumentNullException("Input data cannot be null");

            var doctor = await _doctorRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Doctor not found");

            var createdDoctorDto = _mapper.Map<CreateDoctorDto>(updateDoctorDto);

            await ValidateDoctorData(createdDoctorDto);
            _mapper.Map(updateDoctorDto, doctor);

            if (updateDoctorDto.Image != null && updateDoctorDto.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(doctor.ImagePath))
                {
                    await _fileService.DeleteFileAsync(doctor.ImagePath!);
                }
            }

            doctor.ImagePath = updateDoctorDto.Image != null
          ? _fileService.SaveFile(updateDoctorDto.Image, "Doctors") : null;

            await _doctorRepository.UpdateAsync(id, doctor);
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id) ??
              throw new KeyNotFoundException("Doctor not found");

            if (!string.IsNullOrEmpty(doctor.ImagePath))
            {
                await _fileService.DeleteFileAsync(doctor.ImagePath);
            }
            await _doctorRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialty(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("Specialty cannot be empty.");

            var doctors = await _doctorRepository.GetDoctorsBySpecialty(specialty);

            if (!doctors.Any() || doctors == null)
                throw new KeyNotFoundException($"No doctors found for specialty '{specialty}'.");

            var doctorsDto = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            return doctorsDto;
        }

        public async Task<IEnumerable<DoctorDto>> GetAvailableDoctorsToDay()
        {
            var doctors = await _doctorRepository.GetAvailableDoctorsToDay();

            if (!doctors.Any() || doctors == null)
                throw new KeyNotFoundException("No available doctors today");

            var doctorsDto = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            return doctorsDto;
        }

        public async Task AssignDoctorToDepartmentAsync(int doctorId, int departmentId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId) ??
                throw new KeyNotFoundException("Doctor not found");

            if (doctor.DepartmentId == departmentId)
                throw new InvalidOperationException("Doctor is already in this department.");

            var result = await _doctorRepository.AssignDoctorToDepartment(doctorId, departmentId);
            if (!result) throw new Exception("not Assign!");

        }

        public async Task UpdateDoctorWorkingHoursAsync(int doctorId, string newWorkingHours)
        {
            if (string.IsNullOrWhiteSpace(newWorkingHours) || newWorkingHours.Length > 100)
            {
                throw new ArgumentException("Invalid working hours. It must be non-empty and less than 100 characters");
            }

            var success = await _doctorRepository.UpdateDoctorWorkingHoursAsync(doctorId, newWorkingHours);
            if (!success)
            {
                throw new KeyNotFoundException($"Doctor with ID {doctorId} not found.");
            }
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

            if (!await _doctorRepository.DepartmentExistsAsync(doctorDto.DepartmentId))
            {
                throw new KeyNotFoundException("Department not found!");
            }

            if (!await _doctorRepository.IsPhoneNumberValid(doctorDto.Phone))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }
        }
    }
}
