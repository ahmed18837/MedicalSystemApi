namespace MedicalSystemApi.Models.DTOs.BillItem
{
    public class UpdateBillItemDto
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int BillId { get; set; }
    }
}
