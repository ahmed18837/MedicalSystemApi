using AutoMapper;
using MedicalSystemApi.Models.DTOs.Bill;
using MedicalSystemApi.Models.DTOs.BillMedicalTest;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class BillMedicalTestService : IBillMedicalTestService
    {
        private readonly IBillMedicalTestRepository _billMedicalTestRepository;
        private readonly IMedicalTestRepository _medicalTestRepository;
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public BillMedicalTestService(IBillMedicalTestRepository billMedicalTestRepository, IBillRepository billRepository, IMedicalTestRepository medicalTestRepository, IMapper mapper)
        {
            _billMedicalTestRepository = billMedicalTestRepository;
            _billRepository = billRepository;
            _medicalTestRepository = medicalTestRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BillMedicalTestDto>> GetAllAsync()
        {
            var billMedicalTestListDto = await _billMedicalTestRepository.GetAllWithMedicalTestName() ??
                throw new Exception("There are not BillMedicalTest!");

            return billMedicalTestListDto;
        }

        public async Task<BillMedicalTestDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var billMedicalTestDto = await _billMedicalTestRepository.GetOneWithMedicalTestName(id) ??
                throw new InvalidOperationException("BillMedicalTest not fount!");

            return billMedicalTestDto;
        }


        public async Task AddAsync(CreateBillMedicalTestDto createBillMedicalTestDto)
        {
            if (createBillMedicalTestDto == null) throw new ArgumentNullException("Input data cannot be null");

            await ValidationForMedicalTest(createBillMedicalTestDto);

            var billMedicalTest = _mapper.Map<BillMedicalTest>(createBillMedicalTestDto);
            await _billMedicalTestRepository.AddAsync(billMedicalTest);
        }

        public async Task UpdateAsync(int id, UpdateBillMedicalTestDto updateBillMedicalTestDto)
        {
            var billMedicalTest = await _billMedicalTestRepository.GetByIdAsync(id) ??
                   throw new InvalidOperationException("BillMedicalTest not fount!");

            await ValidationForMedicalTest(updateBillMedicalTestDto);

            var updatedBillMedicalTest = _mapper.Map(updateBillMedicalTestDto, billMedicalTest);

            await _billMedicalTestRepository.UpdateAsync(id, updatedBillMedicalTest);
        }

        public async Task DeleteAsync(int id)
        {
            var billMedicalTest = await _billMedicalTestRepository.GetByIdAsync(id) ??
               throw new InvalidOperationException("BillMedicalTest not fount!");
            await _billMedicalTestRepository.DeleteAsync(id);
        }

        public async Task ValidationForMedicalTest(CreateBillMedicalTestDto createBillMedicalTestDto)
        {
            if (!await _billMedicalTestRepository.BillExistsAsync(createBillMedicalTestDto.BillId))
            {
                throw new KeyNotFoundException("Bill not found!");
            }
            if (!await _billMedicalTestRepository.MedicalTestExistsAsync(createBillMedicalTestDto.MedicalTestId))
            {
                throw new KeyNotFoundException("MedicalTest not found!");
            }
            if (createBillMedicalTestDto.TestCost <= 0)
                throw new ArgumentException("TestCost must be greater than zero");
        }

        public async Task<IEnumerable<BillMedicalTestDto>> GetTestsByBillIdAsync(int billId)
        {
            var bill = await _billRepository.GetByIdAsync(billId);
            if (bill == null)
                throw new ArgumentException($"Bill with ID {billId} does not exist.");


            var testsDto = await _billMedicalTestRepository.GetTestsByBillIdAsync(billId);
            if (!testsDto.Any())
                throw new InvalidOperationException($"No medical tests found for Bill ID {billId}");

            return testsDto;
        }

        public async Task<IEnumerable<BillDto>> GetBillsForMedicalTestAsync(int testId)
        {
            var medicalTest = await _medicalTestRepository.GetByIdAsync(testId);
            if (medicalTest == null)
                throw new ArgumentException($"Medical test with ID {testId} does not exist.");

            // **2️⃣ Fetch Bills for the Given Test**
            var bills = await _billMedicalTestRepository.GetBillsByTestIdAsync(testId);
            if (!bills.Any())
                throw new InvalidOperationException($"No bills found for Medical Test ID {testId}");
            var billsDto = _mapper.Map<IEnumerable<BillDto>>(bills);
            return billsDto;
        }

        public async Task UpdateTestCostAsync(int id, decimal newCost)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid BillMedicalTest ID");

            if (newCost <= 0)
                throw new ArgumentException("Test cost must be greater than zero");

            var updated = await _billMedicalTestRepository.UpdateTestCostAsync(id, newCost);

            if (!updated)
                throw new KeyNotFoundException($"No record found for BillMedicalTest ID {id}");
        }

        public async Task<IEnumerable<BillMedicalTestDto>> GetFilteredBillMedicalTestsAsync(BillMedicalTestFilterDto filterDto)
        {
            var billMedicalTestsDto = await _billMedicalTestRepository.GetFilteredBillMedicalTestsAsync(filterDto);
            if (billMedicalTestsDto == null && !billMedicalTestsDto!.Any())
                throw new Exception("No billMedicalTests found matching the given criteria.");

            return billMedicalTestsDto;
        }
    }
}
