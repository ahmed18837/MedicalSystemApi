using AutoMapper;
using MedicalSystemApi.Models.DTOs.Patient;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class PatientService(IPatientRepository patientRepository, IMapper mapper) : IPatientService
    {
        private readonly IPatientRepository _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patientList = await _patientRepository.GetAllAsync() ??
               throw new Exception("There are not Patients!");

            var patientListDto = _mapper.Map<IEnumerable<PatientDto>>(patientList); // ابطىء نسبيا ولكن سهل فى التعديل
            return patientListDto;
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var patient = await _patientRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Patient not fount");

            var patientDto = _mapper.Map<PatientDto>(patient);
            return patientDto;
        }

        public async Task AddAsync(CreatePatientDto createPatientDto)
        {
            if (createPatientDto == null) throw new ArgumentNullException("Input data cannot be null");

            // التحقق من صحة البيانات

            if (string.IsNullOrWhiteSpace(createPatientDto.FullName))
            {
                throw new ArgumentException("Full Name cannot be null or empty.");
            }

            if (!await _patientRepository.IsPhoneNumberValid(createPatientDto.Phone))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }

            var patient = _mapper.Map<Patient>(createPatientDto);

            patient.MedicalHistoryDate = DateTime.Now;

            await _patientRepository.AddAsync(patient);
        }

        public async Task UpdateAsync(int id, UpdatePatientDto updatePatientDto)
        {
            if (updatePatientDto == null) throw new ArgumentNullException("Input data cannot be null");

            var patient = await _patientRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Patient not found");

            if (string.IsNullOrWhiteSpace(updatePatientDto.FullName))
            {
                throw new ArgumentException("Full Name cannot be null or empty.");
            }

            if (!await _patientRepository.IsPhoneNumberValid(updatePatientDto.Phone))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }

            var patientUpdated = _mapper.Map(updatePatientDto, patient);

            patient.MedicalHistoryDate = DateTime.Now;

            await _patientRepository.UpdateAsync(id, patientUpdated);
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id) ??
              throw new KeyNotFoundException("Patient not found");

            await _patientRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsByGenderAsync(string gender)
        {
            var patients = await _patientRepository.GetPatientsByGenderAsync(gender);
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsByAgeRangeAsync(int minAge, int maxAge)
        {
            if (minAge < 0 || maxAge < 0 || minAge > maxAge)
                throw new ArgumentException("Invalid age range: minAge must be ≤ maxAge and non-negative.");

            var patients = await _patientRepository.GetPatientsByAgeRangeAsync(minAge, maxAge);

            if (patients == null || !patients.Any())
                throw new KeyNotFoundException($"No patients found between {minAge} and {maxAge} years old.");
            var patientsDto = _mapper.Map<IEnumerable<PatientDto>>(patients);
            return patientsDto;
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsWithAppointmentsAsync()
        {
            var patients = await _patientRepository.GetPatientsWithAppointmentsAsync();

            if (patients == null || !patients.Any())
                throw new KeyNotFoundException("No patients with appointments found");

            var patientsDto = _mapper.Map<IEnumerable<PatientDto>>(patients);
            return patientsDto;
        }

        public async Task<Dictionary<string, int>> GetPatientCountByGenderAsync()
        {
            var genderCounts = await _patientRepository.GetPatientCountByGenderAsync();

            if (genderCounts == null || genderCounts.Count == 0)
                throw new KeyNotFoundException("No patient gender data found");

            return genderCounts;
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsAdmittedInLastYearAsync(int year)
        {
            int currentYear = DateTime.Now.Year;
            if (year > currentYear)
            {
                throw new ArgumentException("Year cannot be in the future.");
            }
            if (year < currentYear - 100)
            {
                throw new ArgumentException("Year is too old. Please provide a valid year.");
            }
            var patients = await _patientRepository.GetPatientsAdmittedInLastYearAsync(year);

            if (patients == null || !patients.Any())
            {
                throw new KeyNotFoundException($"No patients found for the year {year}");
            }
            var patientsDto = _mapper.Map<IEnumerable<PatientDto>>(patients);
            return patientsDto;
        }

        public async Task<IEnumerable<PatientDto>> SearchPatientsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Patient name is required for searching.");

            var patients = await _patientRepository.SearchPatientsByNameAsync(name);

            if (patients == null || !patients.Any())
                throw new KeyNotFoundException($"No patients found matching: {name}");

            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task UpdatePatientPhoneAsync(int patientId, string newPhone)
        {
            if (string.IsNullOrWhiteSpace(newPhone))
                throw new ArgumentException("Phone number is required.");

            var patient = await _patientRepository.GetByIdAsync(patientId) ??
               throw new InvalidOperationException("Patient not fount");


            if (!await _patientRepository.IsPhoneNumberValid(newPhone))
            {
                throw new InvalidOperationException("Invalid Phone Number!");
            }

            await _patientRepository.UpdatePatientPhoneAsync(patientId, newPhone);
        }

        public async Task<IEnumerable<PatientDto>> GetFilteredPatientsAsync(PatientFilterDto filterDto)
        {
            var patients = await _patientRepository.GetFilteredPatientsAsync(filterDto);

            if (patients == null && !patients.Any())
                throw new Exception("No patients found matching the given criteria.");

            return patients.Select(p => new PatientDto
            {
                //Id = p.Id,
                FullName = p.FullName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                Address = p.Address,
                Phone = p.Phone,
                MedicalHistoryDate = p.MedicalHistoryDate
            }).ToList();
        }
    }
}
