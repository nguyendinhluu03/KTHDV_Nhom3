namespace Baithuchanh2.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Liên kết với Order
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // Navigation property
        public Orders Order { get; set; }
    }
}
