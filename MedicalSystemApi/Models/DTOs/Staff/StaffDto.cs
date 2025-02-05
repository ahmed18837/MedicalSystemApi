namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class StaffDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string RoleStaff { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? HireDate { get; set; }
        public string DepartmentName { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
