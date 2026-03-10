namespace BackendAssementWebAPIApp.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime DateOfSale { get; set; }
        public string Region { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public List<OrderDetailDto> Details { get; set; } = new();
    }
}