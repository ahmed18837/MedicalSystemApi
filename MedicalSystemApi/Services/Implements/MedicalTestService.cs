using AutoMapper;
using MedicalSystemApi.Models.DTOs.MedicalTest;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class MedicalTestService(IMedicalTestRepository medicalTestRepository, IMapper mapper) : IMedicalTestService
    {
        private readonly IMedicalTestRepository _medicalTestRepository = medicalTestRepository;
        private readonly IMapper _mapper = mapper;

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

        public async Task<IEnumerable<MedicalTestDto>> GetExpensiveTests(decimal minCost)
        {
            if (minCost < 0)
                throw new ArgumentException("Cost cannot be negative");

            var medicalTest = await _medicalTestRepository.GetExpensiveTests(minCost);
            var medicalTestDto = _mapper.Map<IEnumerable<MedicalTestDto>>(medicalTest);
            return medicalTestDto;
        }

        public async Task<IEnumerable<MedicalTestDto>> SearchMedicalTests(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("Search term cannot be empty");

            var medicalTest = await _medicalTestRepository.SearchMedicalTests(searchTerm);
            var medicalTestDto = _mapper.Map<IEnumerable<MedicalTestDto>>(medicalTest);
            return medicalTestDto;
        }

        public async Task AssignMedicalTestToBill(int testId, int billId)
        {
            if (testId <= 0 || billId <= 0)
                throw new ArgumentException("Test ID and Bill ID must be valid.");

            await _medicalTestRepository.AssignMedicalTestToBill(testId, billId);
        }

        public async Task UpdateMedicalTestCost(int testId, decimal newCost)
        {
            if (newCost <= 0)
                throw new ArgumentException("Cost must be greater than zero.");
            if (testId <= 0)
                throw new ArgumentException("ID not be valid");

            await _medicalTestRepository.UpdateMedicalTestCost(testId, newCost);
        }

        public async Task<IEnumerable<MedicalTestDto>> GetFilteredMedicalTestsAsync(MedicalTestFilterDto filterDto)
        {
            var medicalTests = await _medicalTestRepository.GetFilteredMedicalTestsAsync(filterDto);

            if (medicalTests == null && !medicalTests.Any())
                throw new Exception("No medicalTests found matching the given criteria.");
            var medicalTestsDto = _mapper.Map<IEnumerable<MedicalTestDto>>(medicalTests);

            return medicalTestsDto;
        }
    }
}
