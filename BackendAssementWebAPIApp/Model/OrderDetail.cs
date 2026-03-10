namespace BackendAssementWebAPIApp.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public string ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int QuantitySold { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingCost { get; set; }
    }
}