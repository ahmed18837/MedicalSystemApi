namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class CreateStaffDto
    {
        public string FullName { get; set; }
        public string RoleStaff { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
    }
}
