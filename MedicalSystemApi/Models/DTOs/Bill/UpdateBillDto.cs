namespace MedicalSystemApi.Models.DTOs.Bill
{
    public class UpdateBillDto
    {
        public decimal TotalAmount { get; set; }
        public int PatientId { get; set; }
    }
}
