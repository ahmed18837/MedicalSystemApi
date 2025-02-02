namespace MedicalSystemApi.Models.DTOs.BillItem
{
    public class BillItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int BillId { get; set; }
    }
}
