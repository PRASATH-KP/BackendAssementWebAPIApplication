namespace BackendAssementWebAPIApp.Model.NewFolder
{
    public class SalesDTO
    {
        public class SalesCsvDto
        {
            public int OrderID { get; set; }
            public int ProductID { get; set; }
            public int CustomerID { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public string Category { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
            public DateTime DateOfSale { get; set; }
            public int QuantitySold { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Discount { get; set; }
            public decimal ShippingCost { get; set; }
            public string PaymentMethod { get; set; } = string.Empty;
            public string CustomerName { get; set; } = string.Empty;
            public string CustomerEmail { get; set; } = string.Empty;
            public string CustomerAddress { get; set; } = string.Empty;
        }
    }
}
