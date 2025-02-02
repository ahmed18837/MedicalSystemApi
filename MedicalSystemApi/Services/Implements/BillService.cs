using AutoMapper;
using MedicalSystemApi.Models.DTOs.Bill;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public BillService(IBillRepository billRepository, IMapper mapper)
        {
            _billRepository = billRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BillDto>> GetAllAsync()
        {
            var billList = await _billRepository.GetAllAsync() ??
                 throw new Exception("There are not Bill!");

            var billListDto = _mapper.Map<IEnumerable<BillDto>>(billList);
            return billListDto;
        }

        public async Task<BillDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var bill = await _billRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Bill not fount!");

            var billDto = _mapper.Map<BillDto>(bill);
            return billDto;
        }

        public async Task AddAsync(CreateBillDto createBillDto)
        {
            if (createBillDto == null) throw new ArgumentNullException("Input data cannot be null");

            if (!await _billRepository.PatientExistsAsync(createBillDto.PatientId))
            {
                throw new KeyNotFoundException("Patient not found!");
            }
            if (createBillDto.TotalAmount <= 0)
                throw new ArgumentException("TotalAmount must be greater than zero");
            createBillDto.DateIssued = DateTime.Now;
            var bill = _mapper.Map<Bill>(createBillDto);
            await _billRepository.AddAsync(bill);
        }

        public async Task UpdateAsync(int id, UpdateBillDto updateBillDto)
        {
            var bill = await _billRepository.GetByIdAsync(id) ??
                     throw new InvalidOperationException("Bill not fount!");
            if (!await _billRepository.PatientExistsAsync(updateBillDto.PatientId))
            {
                throw new KeyNotFoundException("Patient not found!");
            }
            if (updateBillDto.TotalAmount <= 0)
                throw new ArgumentException("TotalAmount must be greater than zero");
            //updateBillDto.DateIssued = DateTime.Now;

            var updatedBill = _mapper.Map(updateBillDto, bill);

            await _billRepository.UpdateAsync(id, updatedBill);
        }

        public async Task DeleteAsync(int id)
        {
            var bill = await _billRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Bill not fount!");
            await _billRepository.DeleteAsync(id);
        }
    }
}
