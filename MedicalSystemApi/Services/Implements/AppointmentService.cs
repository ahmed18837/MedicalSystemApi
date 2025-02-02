using AutoMapper;
using MedicalSystemApi.Models.DTOs.Appointment;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointmentListDto = await _appointmentRepository.GetAllWithDoctorAndPatientAndStaff() ??
                     throw new Exception("There are not Appointment!");



            // var appointmentListDto = _mapper.Map<IEnumerable<AppointmentDto>>(appointmentList);
            return appointmentListDto;
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be greater than zero");

            var appointmentDto = await _appointmentRepository.GetByIdWithDoctorAndPatientAndStaff(id) ??
                throw new InvalidOperationException("Appointment not fount!");

            //var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            return appointmentDto;
        }


        public async Task UpdateAsync(int id, UpdateAppointmentDto updateAppointmentDto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Appointment not fount!");

            await ValidationForAppointment(updateAppointmentDto);

            var updatedAppointment = _mapper.Map(updateAppointmentDto, appointment);

            await _appointmentRepository.UpdateAsync(id, updatedAppointment);
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id) ??
               throw new InvalidOperationException("Appointment not fount!");
            await _appointmentRepository.DeleteAsync(id);
        }

        public async Task ValidationForAppointment(UpdateAppointmentDto updateAppointmentDto)
        {
            if (!await _appointmentRepository.PatientExistsAsync(updateAppointmentDto.PatientId))
            {
                throw new KeyNotFoundException("Patient not found!");
            }
            if (!await _appointmentRepository.DoctorExistsAsync(updateAppointmentDto.DoctorId))
            {
                throw new KeyNotFoundException("Doctor not found!");
            }
            if (!await _appointmentRepository.StaffExistsAsync(updateAppointmentDto.StaffId))
            {
                throw new KeyNotFoundException("Staff not found!");
            }
        }

        public async Task AddAsync(CreateAppointmentDto createAppointmentDto)
        {
            if (createAppointmentDto == null) throw new ArgumentNullException("Input data cannot be null");

            createAppointmentDto.Status = "Pending";

            var billItem = _mapper.Map<Appointment>(createAppointmentDto);
            await _appointmentRepository.AddAsync(billItem);
        }
    }
}
