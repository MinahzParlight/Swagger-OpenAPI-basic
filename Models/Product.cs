namespace ProductService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty; // Mã vạch/Mã kho
        public decimal Price { get; set; }
        public int StockQuantity { get; set; } // Số lượng tồn kho
    }
}