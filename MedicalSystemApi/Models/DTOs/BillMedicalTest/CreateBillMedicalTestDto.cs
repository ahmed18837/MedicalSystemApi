namespace MedicalSystemApi.Models.DTOs.BillMedicalTest
{
    public class CreateBillMedicalTestDto
    {
        public int BillId { get; set; }
        public int MedicalTestId { get; set; }
        public decimal TestCost { get; set; }
    }
}
