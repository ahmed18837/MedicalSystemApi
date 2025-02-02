namespace MedicalSystemApi.Models.DTOs.Bill
{
    public class CreateBillDto
    {
        public DateTime DateIssued { get; set; }
        public decimal TotalAmount { get; set; }
        public int PatientId { get; set; }
    }
}
