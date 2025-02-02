using AutoMapper;
using MedicalSystemApi.Models.DTOs.BillItem;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class BillItemService : IBillItemService
    {
        private readonly IBillItemRepository _billItemRepository;
        private readonly IMapper _mapper;

        public BillItemService(IBillItemRepository billItemRepository, IMapper mapper)
        {
            _billItemRepository = billItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BillItemDto>> GetAllAsync()
        {
            var billItemList = await _billItemRepository.GetAllAsync() ??
                 throw new Exception("There are not BillItem!");

            var billItemListDto = _mapper.Map<IEnumerable<BillItemDto>>(billItemList);
            return billItemListDto;
        }

        public async Task<BillItemDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var billItem = await _billItemRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("BillItem not fount!");

            var billItemDto = _mapper.Map<BillItemDto>(billItem);
            return billItemDto;
        }

        public async Task AddAsync(CreateBillItemDto createBillItemDto)
        {
            if (createBillItemDto == null) throw new ArgumentNullException("Input data cannot be null");

            await ValidationForBillItem(createBillItemDto);

            var billItem = _mapper.Map<BillItem>(createBillItemDto);
            await _billItemRepository.AddAsync(billItem);
        }

        public async Task DeleteAsync(int id)
        {
            var billItem = await _billItemRepository.GetByIdAsync(id) ??
               throw new InvalidOperationException("BillItem not fount!");
            await _billItemRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(int id, UpdateBillItemDto updateBillItemDto)
        {
            var billItem = await _billItemRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("BillItem not fount!");

            await ValidationForBillItem(updateBillItemDto);

            var updatedBillItem = _mapper.Map(updateBillItemDto, billItem);

            await _billItemRepository.UpdateAsync(id, updatedBillItem);
        }

        public async Task ValidationForBillItem(UpdateBillItemDto updateBillItemDto)
        {
            if (string.IsNullOrWhiteSpace(updateBillItemDto.ItemName))
            {
                throw new ArgumentException("ItemName cannot be null or empty.");
            }

            if (!await _billItemRepository.BillExistsAsync(updateBillItemDto.BillId))
            {
                throw new KeyNotFoundException("Bill not found!");
            }

            if (updateBillItemDto.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            if (updateBillItemDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");
        }
    }
}
