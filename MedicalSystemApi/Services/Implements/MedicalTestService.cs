using AutoMapper;
using MedicalSystemApi.Models.DTOs.MedicalTest;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class MedicalTestService : IMedicalTestService
    {
        private readonly IMedicalTestRepository _medicalTestRepository;
        private readonly IMapper _mapper;

        public MedicalTestService(IMedicalTestRepository medicalTestRepository, IMapper mapper)
        {
            _medicalTestRepository = medicalTestRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicalTestDto>> GetAllAsync()
        {
            var medicalTestList = await _medicalTestRepository.GetAllAsync() ??
                 throw new Exception("There are not MedicalTest!");

            var medicalTestListDto = _mapper.Map<IEnumerable<MedicalTestDto>>(medicalTestList); // ابطىء نسبيا ولكن سهل فى التعديل
            return medicalTestListDto;
        }

        public async Task<MedicalTestDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var medicalTest = await _medicalTestRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("MedicalTest not fount");

            var medicalTestDto = _mapper.Map<MedicalTestDto>(medicalTest);
            return medicalTestDto;
        }

        public async Task AddAsync(CreateMedicalTestDto createMedicalTestDto)
        {
            if (createMedicalTestDto == null) throw new ArgumentNullException("Input data cannot be null");
            if (string.IsNullOrWhiteSpace(createMedicalTestDto.TestName))
            {
                throw new ArgumentException("Test Name cannot be null or empty.");
            }
            var medicalTest = _mapper.Map<MedicalTest>(createMedicalTestDto);

            await _medicalTestRepository.AddAsync(medicalTest);
        }

        public async Task UpdateAsync(int id, UpdateMedicalTestDto updateMedicalTestDto)
        {
            if (updateMedicalTestDto == null) throw new ArgumentNullException("Input data cannot be null");

            var medicalTest = await _medicalTestRepository.GetByIdAsync(id) ??
            throw new KeyNotFoundException("MedicalTest not found");
            var medicalTestUpdated = _mapper.Map(updateMedicalTestDto, medicalTest);

            await _medicalTestRepository.UpdateAsync(id, medicalTestUpdated);
        }

        public async Task DeleteAsync(int id)
        {
            var medicalTest = await _medicalTestRepository.GetByIdAsync(id) ??
             throw new KeyNotFoundException("MedicalTest not found");

            await _medicalTestRepository.DeleteAsync(id);
        }
    }
}
