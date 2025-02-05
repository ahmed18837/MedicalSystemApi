using AutoMapper;
using MedicalSystemApi.Models.DTOs.Appointment;
using MedicalSystemApi.Models.DTOs.Bill;
using MedicalSystemApi.Models.DTOs.BillItem;
using MedicalSystemApi.Models.DTOs.BillMedicalTest;
using MedicalSystemApi.Models.DTOs.Department;
using MedicalSystemApi.Models.DTOs.Doctor;
using MedicalSystemApi.Models.DTOs.MedicalRecord;
using MedicalSystemApi.Models.DTOs.MedicalTest;
using MedicalSystemApi.Models.DTOs.Medication;
using MedicalSystemApi.Models.DTOs.Patient;
using MedicalSystemApi.Models.DTOs.Staff;
using MedicalSystemApi.Models.Entities;

namespace MedicalSystemApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Staff, StaffDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
            CreateMap<StaffDto, Staff>()
           .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate ?? DateTime.Now)); // ✅ تعيين قيمة افتراضية
            CreateMap<CreateStaffDto, Staff>();
            CreateMap<Staff, UpdateStaffDto>().ReverseMap();
            CreateMap<UpdateStaffDto, CreateStaffDto>().ReverseMap();
            CreateMap<Staff, PatchStaffDto>().ReverseMap();

            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>().ReverseMap();
            CreateMap<Patient, PatchPatientDto>().ReverseMap();

            CreateMap<Medication, MedicationDto>().ReverseMap();
            CreateMap<CreateMedicationDto, Medication>();
            CreateMap<UpdateMedicationDto, Medication>().ReverseMap();

            CreateMap<MedicalRecord, MedicalRecordDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName ?? string.Empty));
            CreateMap<CreateMedicalRecordDto, MedicalRecord>();
            CreateMap<UpdateMedicalRecordDto, MedicalRecord>().ReverseMap();

            CreateMap<MedicalTest, MedicalTestDto>().ReverseMap();
            CreateMap<CreateMedicalTestDto, MedicalTest>();
            CreateMap<UpdateMedicalTestDto, MedicalTest>();

            CreateMap<Doctor, DoctorDto>()
                 .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name)); // Map DepartmentName
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, CreateDoctorDto>().ReverseMap();
            CreateMap<UpdateDoctorDto, Doctor>();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();

            CreateMap<Bill, BillDto>().ReverseMap();
            CreateMap<CreateBillDto, Bill>();
            CreateMap<UpdateBillDto, Bill>();

            CreateMap<BillMedicalTest, BillMedicalTestDto>()
                .ForMember(dest => dest.MedicalTestName, opt => opt.MapFrom(src => src.MedicalTest.TestName));
            CreateMap<CreateBillMedicalTestDto, BillMedicalTest>();
            CreateMap<UpdateBillMedicalTestDto, BillMedicalTest>();

            CreateMap<BillItem, BillItemDto>().ReverseMap();
            CreateMap<CreateBillItemDto, BillItem>();
            CreateMap<UpdateBillItemDto, BillItem>();
            CreateMap<UpdateBillItemDto, CreateBillItemDto>();

            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));
            CreateMap<UpdateAppointmentDto, Appointment>();

        }
    }
}
