using AutoMapper;
using MedicalSystemApi.Models.DTOs.BillMedicalTest;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class BillMedicalTestService : IBillMedicalTestService
    {
        private readonly IBillMedicalTestRepository _billMedicalTestRepository;
        private readonly IMapper _mapper;

        public BillMedicalTestService(IBillMedicalTestRepository billMedicalTestRepository, IMapper mapper)
        {
            _billMedicalTestRepository = billMedicalTestRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BillMedicalTestDto>> GetAllAsync()
        {
            var billMedicalTestList = await _billMedicalTestRepository.GetAllWithMedicalTestName() ??
                throw new Exception("There are not BillMedicalTest!");

            var billMedicalTestListDto = _mapper.Map<IEnumerable<BillMedicalTestDto>>(billMedicalTestList);
            return billMedicalTestListDto;
        }

        public async Task<BillMedicalTestDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var billMedicalTest = await _billMedicalTestRepository.GetOneWithMedicalTestName(id) ??
                throw new InvalidOperationException("BillMedicalTest not fount!");

            var billMedicalTestDto = _mapper.Map<BillMedicalTestDto>(billMedicalTest);
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

            ValidationForMedicalTest(updateBillMedicalTestDto);

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
    }
}
