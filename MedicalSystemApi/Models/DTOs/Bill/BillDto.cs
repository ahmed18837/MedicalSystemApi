namespace MedicalSystemApi.Models.DTOs.Bill
{
    public class BillDto
    {
        //public int Id { get; set; }
        public DateTime DateIssued { get; set; }
        public decimal TotalAmount { get; set; }
        public int PatientId { get; set; }
    }
}
