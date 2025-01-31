namespace MedicalSystemApi.Models.DTOs.Staff
{
    public class PatchStaffDto
    {
        public string FullName { get; set; }
        public string RoleStaff { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public DateTime HireDate { get; set; }
    }
}
