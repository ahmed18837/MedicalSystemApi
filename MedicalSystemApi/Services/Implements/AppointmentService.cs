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




        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            if (patientId <= 0)
                throw new ArgumentException("Invalid PatientId");

            var appointments = await _appointmentRepository.GetAppointmentsByPatientIdAsync(patientId);

            if (!appointments.Any())
                throw new Exception($"No appointments found for PatientId: {patientId}");

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                Date = a.Date,
                Time = a.Time,
                Status = a.Status,
                Notes = a.Notes,
                PatientName = a.Patient != null ? a.Patient.FullName : "Unknown",
                DoctorName = a.Doctor != null ? a.Doctor.FullName : "Unknown",
                StaffName = a.Staff != null ? a.Staff.FullName : "N/A"
            });
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            if (doctorId <= 0)
                throw new ArgumentException("Invalid DoctorId.");

            var appointments = await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId);

            if (!appointments.Any())
                throw new Exception($"No appointments found for DoctorId: {doctorId}");

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                Date = a.Date,
                Time = a.Time,
                Status = a.Status,
                Notes = a.Notes,
                PatientName = a.Patient != null ? a.Patient.FullName : "Unknown",
                DoctorName = a.Doctor != null ? a.Doctor.FullName : "Unknown",
                StaffName = a.Staff != null ? a.Staff.FullName : "N/A"
            });
        }

        public async Task<string> CheckDoctorAvailabilityAsync(int doctorId, DateTime date, TimeSpan time)
        {
            if (date < DateTime.Today)
                throw new ArgumentException("Date cannot be in the past.");

            bool isBooked = await _appointmentRepository.CheckDoctorAvailabilityAsync(doctorId, date, time);

            return isBooked ? "Doctor is not available at this time." : "Doctor is available.";
        }

        public async Task UpdateAppointmentStatusAsync(int appointmentId, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new Exception("Status cannot be empty");

            if (appointmentId <= 0)
                throw new ArgumentException("Invalid AppointmentId");

            var validStatuses = new List<string> { "Pending", "Confirmed", "Cancelled", "Completed" };
            if (!validStatuses.Contains(status))
                throw new ArgumentException("Invalid status. Allowed values: Pending, Confirmed, Cancelled, Completed");

            await _appointmentRepository.UpdateAppointmentStatusAsync(appointmentId, status);
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetUpcomingAppointmentsAsync();

            if (!appointments.Any())
                throw new Exception("No upcoming appointments found.");

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                Date = a.Date,
                Time = a.Time,
                Status = a.Status,
                Notes = a.Notes,
                PatientName = a.Patient != null ? a.Patient.FullName : "Unknown",
                DoctorName = a.Doctor != null ? a.Doctor.FullName : "Unknown",
                StaffName = a.Staff != null ? a.Staff.FullName : "N/A"
            });
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

        public async Task CreateAppointmentAsync(CreateAppointmentDto createDto)
        {
            var now = DateTime.Now;

            // Default to current date/time if missing
            createDto.Date = createDto.Date == default ? now.Date : createDto.Date;
            //createDto.Time = createDto.Time == default ? now.TimeOfDay : createDto.Time;



            if (createDto.Date < DateTime.Today)
                throw new ArgumentException("Appointment date cannot be in the past");

            // Validate Status
            var allowedStatuses = new List<string> { "Pending", "Confirmed", "Cancelled" };
            if (!string.IsNullOrEmpty(createDto.Status) && !allowedStatuses.Contains(createDto.Status))
                throw new ArgumentException("Invalid status. Allowed values: Pending, Confirmed, Cancelled.");

            bool patientExists = await _appointmentRepository.PatientExistsAsync(createDto.PatientId);
            if (!patientExists)
                throw new KeyNotFoundException("Patient not found.");

            // Validate DoctorId
            bool doctorExists = await _appointmentRepository.DoctorExistsAsync(createDto.DoctorId);
            if (!doctorExists)
                throw new KeyNotFoundException("Doctor not found.");

            // Validate StaffId (Optional)
            if (createDto.StaffId.HasValue)
            {
                bool staffExists = await _appointmentRepository.StaffExistsAsync(createDto.StaffId.Value);
                if (!staffExists)
                    throw new KeyNotFoundException("Staff not found.");
            }

            bool isDoctorBooked = await _appointmentRepository.CheckDoctorAvailabilityAsync(createDto.DoctorId, createDto.Date, createDto.Time);
            if (isDoctorBooked)
                throw new InvalidOperationException("The doctor is not available at this time");

            var appointment = new Appointment
            {
                Date = createDto.Date,
                Time = createDto.Time,
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                StaffId = createDto.StaffId,
                Notes = createDto.Notes,
                Status = createDto.Status ?? "Pending" // Default if null
            };

            await _appointmentRepository.AddAsync(appointment);
        }

    }
}
