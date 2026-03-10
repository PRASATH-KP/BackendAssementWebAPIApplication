using System.ComponentModel.DataAnnotations;

namespace BackendAssementWebAPIApp.Model
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; }     // Product ID (Primary Key)
        public string Name { get; set; } = string.Empty;   // Product Name
        public string Category { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }

        // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}