using AutoMapper;
using MedicalSystemApi.Models.DTOs.MedicalRecord;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMapper _mapper;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetAllAsync()
        {
            var medicalRecordList = await _medicalRecordRepository.GetAllWithDoctorName() ??
                throw new Exception("There are not MedicationRecord!");

            var medicalRecordListDto = _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecordList); // ابطىء نسبيا ولكن سهل فى التعديل
            return medicalRecordListDto;
        }

        public async Task<MedicalRecordDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var medicalRecord = await _medicalRecordRepository.GetMedicalRecordWithDoctorName(id) ??
                throw new InvalidOperationException("MedicationRecord not fount");

            var medicalRecordDto = _mapper.Map<MedicalRecordDto>(medicalRecord);
            return medicalRecordDto;
        }

        public async Task AddAsync(CreateMedicalRecordDto createMedicalRecordDto)
        {
            if (createMedicalRecordDto == null) throw new ArgumentNullException("Input data cannot be null");

            // التحقق من صحة البيانات
            if (!await _medicalRecordRepository.PatientIdExistsAsync(createMedicalRecordDto.PatientId))
                throw new KeyNotFoundException("Patient not found!");

            if (!await _medicalRecordRepository.DoctorIdExistsAsync(createMedicalRecordDto.DoctorId))
                throw new KeyNotFoundException("Doctor not found!");

            var medicalRecord = _mapper.Map<MedicalRecord>(createMedicalRecordDto);

            await _medicalRecordRepository.AddAsync(medicalRecord);
        }

        public async Task UpdateAsync(int id, UpdateMedicalRecordDto updateMedicalRecordDto)
        {
            if (updateMedicalRecordDto == null) throw new ArgumentNullException("Input data cannot be null");


            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id) ??
                throw new KeyNotFoundException("MedicalRecord not found");
            // التحقق من صحة البيانات
            if (!await _medicalRecordRepository.PatientIdExistsAsync(updateMedicalRecordDto.PatientId))
                throw new KeyNotFoundException("Patient not found!");

            if (!await _medicalRecordRepository.DoctorIdExistsAsync(updateMedicalRecordDto.DoctorId))
                throw new KeyNotFoundException("Doctor not found!");

            var medicalRecordUpdated = _mapper.Map(updateMedicalRecordDto, medicalRecord);
            await _medicalRecordRepository.UpdateAsync(id, medicalRecordUpdated);
        }

        public async Task DeleteAsync(int id)
        {
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id) ??
              throw new KeyNotFoundException("MedicationRecord not found");

            await _medicalRecordRepository.DeleteAsync(id);
        }

        public async Task UpdateDiagnosisAndPrescriptions(int recordId, string diagnosis, string prescriptions)
        {

            if (string.IsNullOrWhiteSpace(diagnosis) || string.IsNullOrWhiteSpace(prescriptions))
                throw new ArgumentException("Diagnosis and prescriptions cannot be empty.");

            bool isUpdated = await _medicalRecordRepository.UpdateDiagnosisAndPrescriptions(recordId, diagnosis, prescriptions);

            if (!isUpdated)
                throw new KeyNotFoundException($"Medical record with ID {recordId} not found");


            await _medicalRecordRepository.UpdateDiagnosisAndPrescriptions(recordId, diagnosis, prescriptions);
        }



        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalHistoryByPatientIdAndDoctorId(int patientId, int doctorId)
        {
            var history = await _medicalRecordRepository.GetMedicalHistoryByPatientIdAndDoctorId(patientId, doctorId);

            if (!history.Any())
                throw new KeyNotFoundException($"No medical records found for patient ID {patientId} and doctor ID {doctorId}.");
            var historyDto = _mapper.Map<IEnumerable<MedicalRecordDto>>(history);
            return historyDto;
        }
    }
}
