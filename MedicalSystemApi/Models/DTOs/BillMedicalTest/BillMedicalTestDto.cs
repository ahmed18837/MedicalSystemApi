namespace MedicalSystemApi.Models.DTOs.BillMedicalTest
{
    public class BillMedicalTestDto
    {
        //public int Id { get; set; }
        public int BillId { get; set; }
        public string MedicalTestName { get; set; }
        public decimal TestCost { get; set; }
    }
}
