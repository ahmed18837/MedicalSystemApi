namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class UpdateStaffDto : CreateStaffDto
    {
        public DateTime HireDate { get; set; }
    }
}
