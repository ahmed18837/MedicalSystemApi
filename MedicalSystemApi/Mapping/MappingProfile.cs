using AutoMapper;
using MedicalSystemApi.Models.DTOs.MedicalRecord;
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
            CreateMap<Staff, StaffDto>();
            //.ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
            CreateMap<StaffDto, Staff>()
           .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate ?? DateTime.Now)); // ✅ تعيين قيمة افتراضية
            CreateMap<CreateStaffDto, Staff>();
            CreateMap<Staff, UpdateStaffDto>().ReverseMap();
            CreateMap<UpdateStaffDto, CreateStaffDto>().ReverseMap();
            CreateMap<UpdateStaffDto, PatchStaffDto>().ReverseMap();
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
        }
    }
}
