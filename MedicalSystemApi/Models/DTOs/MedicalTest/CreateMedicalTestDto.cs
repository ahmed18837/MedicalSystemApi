namespace MedicalSystemApi.Models.DTOs.MedicalTest
{
    public class CreateMedicalTestDto
    {
        public string TestName { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
    }
}
