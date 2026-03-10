using System.ComponentModel.DataAnnotations;

namespace BackendAssementWebAPIApp.Model
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; }           // Customer ID (Primary Key)
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}