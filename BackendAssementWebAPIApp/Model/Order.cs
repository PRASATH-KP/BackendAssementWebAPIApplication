using System.ComponentModel.DataAnnotations;

namespace BackendAssementWebAPIApp.Model
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }                       // Order ID (Primary Key)
        public string CustomerId { get; set; }  
        // Foreign key to Customer
        public Customer Customer { get; set; } = null!;
        public DateTime DateOfSale { get; set; }
        public string Region { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;

        // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}