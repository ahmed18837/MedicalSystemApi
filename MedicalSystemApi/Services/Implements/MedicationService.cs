using AutoMapper;
using MedicalSystemApi.Models.DTOs.Medication;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _medicationRepository;
        private readonly IMapper _mapper;

        public MedicationService(IMedicationRepository medicationRepository, IMapper mapper)
        {
            _medicationRepository = medicationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicationDto>> GetAllAsync()
        {
            var medicationList = await _medicationRepository.GetAllAsync() ??
               throw new Exception("There are not Medication!");

            var medicationListDto = _mapper.Map<IEnumerable<MedicationDto>>(medicationList); // ابطىء نسبيا ولكن سهل فى التعديل
            return medicationListDto;
        }

        public async Task<MedicationDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var medication = await _medicationRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Medication not fount");

            var medicationDto = _mapper.Map<MedicationDto>(medication);
            return medicationDto;
        }

        public async Task AddAsync(CreateMedicationDto createMedicationDto)
        {
            if (createMedicationDto == null) throw new ArgumentNullException("Input data cannot be null");

            // التحقق من صحة البيانات
            if (string.IsNullOrWhiteSpace(createMedicationDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }
            if (!await _medicationRepository.MedicalRecordIdExistsAsync(createMedicationDto.MedicalRecordId))
                throw new KeyNotFoundException("MedicalRecord not found!");

            var medication = _mapper.Map<Medication>(createMedicationDto);

            await _medicationRepository.AddAsync(medication);
        }

        public async Task UpdateAsync(int id, UpdateMedicationDto updateMedicationDto)
        {
            if (updateMedicationDto == null) throw new ArgumentNullException("Input data cannot be null");

            var medication = await _medicationRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("Medication not found");

            if (string.IsNullOrWhiteSpace(updateMedicationDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }

            if (!await _medicationRepository.MedicalRecordIdExistsAsync(updateMedicationDto.MedicalRecordId))
                throw new KeyNotFoundException("MedicalRecord not found!");

            var medicationUpdated = _mapper.Map(updateMedicationDto, medication);

            await _medicationRepository.UpdateAsync(id, medicationUpdated);
        }

        public async Task DeleteAsync(int id)
        {
            var medication = await _medicationRepository.GetByIdAsync(id) ??
              throw new KeyNotFoundException("Medication not found");

            await _medicationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MedicationDto>> GetMedicationsByDosageRangeAsync(string minDosage, string maxDosage)
        {
            if (string.IsNullOrEmpty(minDosage) || string.IsNullOrEmpty(maxDosage))
                throw new ArgumentException("Dosage values cannot be null or empty.");

            var medications = await _medicationRepository.GetMedicationsByDosageRangeAsync(minDosage, maxDosage);
            return _mapper.Map<IEnumerable<MedicationDto>>(medications);
        }

        public async Task UpdateMedicationInstructionsAsync(int id, string instructions)
        {
            if (string.IsNullOrWhiteSpace(instructions))
                throw new ArgumentException("Instructions cannot be empty.");

            await _medicationRepository.UpdateMedicationInstructionsAsync(id, instructions);

        }

        public async Task<Dictionary<string, int>> GetMedicationStatisticsAsync()
        {
            var statistics = await _medicationRepository.GetMedicationStatisticsAsync();
            if (statistics == null) throw new Exception("Not Found!");
            return statistics;
        }
    }
}
