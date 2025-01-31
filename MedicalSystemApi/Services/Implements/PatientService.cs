using AutoMapper;
using MedicalSystemApi.Models.DTOs.Patient;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

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
    }
}
