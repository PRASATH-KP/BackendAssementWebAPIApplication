using BackendAssementWebAPIApp.DbContextModel;
using BackendAssementWebAPIApp.Dto;
using BackendAssementWebAPIApp.Model;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SalesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("UploadCsv")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        using var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            BadDataFound = null,
            MissingFieldFound = null,
            HeaderValidated = null
        };

        using var csv = new CsvReader(reader, config);
        csv.Read();
        csv.ReadHeader();

        var customers = new Dictionary<string, Customer>();
        var products = new Dictionary<string, Product>();
        var orders = new Dictionary<string, Order>();
        var orderDetails = new List<OrderDetail>();

        while (csv.Read())
        {
            string customerId = csv.GetField("Customer ID");
            string productId = csv.GetField("Product ID");
            string orderId = csv.GetField("Order ID");

            // Add unique customers
            if (!customers.ContainsKey(customerId))
            {
                customers[customerId] = new Customer
                {
                    CustomerId = customerId,
                    Name = csv.GetField("Customer Name"),
                    Email = csv.GetField("Customer Email"),
                    Address = csv.GetField("Customer Address")
                };
            }

            // Add unique products
            if (!products.ContainsKey(productId))
            {
                products[productId] = new Product
                {
                    ProductId = productId,
                    Name = csv.GetField("Product Name"),
                    Category = csv.GetField("Category"),
                    UnitPrice = csv.GetField<decimal>("Unit Price")
                };
            }

            // Add unique orders
            if (!orders.ContainsKey(orderId))
            {
                orders[orderId] = new Order
                {
                    OrderId = orderId,
                    CustomerId = customerId,
                    DateOfSale = csv.GetField<DateTime>("Date of Sale"),
                    Region = csv.GetField("Region"),
                    PaymentMethod = csv.GetField("Payment Method")
                };
            }

            // Add order details
            orderDetails.Add(new OrderDetail
            {
                OrderId = orderId,
                ProductId = productId,
                QuantitySold = csv.GetField<int>("Quantity Sold"),
                Discount = csv.GetField<decimal>("Discount"),
                ShippingCost = csv.GetField<decimal>("Shipping Cost")
            });
        }

        // Insert into database
        await _context.Customers.AddRangeAsync(customers.Values);
        await _context.Products.AddRangeAsync(products.Values);
        await _context.Orders.AddRangeAsync(orders.Values);
        await _context.OrderDetails.AddRangeAsync(orderDetails);

        await _context.SaveChangesAsync();

        return Ok(new { Message = "CSV uploaded successfully" });
    }

    [HttpGet("TopProducts")]
    public async Task<IActionResult> GetTopProducts(
    DateTime startDate,
    DateTime endDate,
    int topN = 5,
    string? category = null,
    string? region = null)
    {
        // Base query for order details joined with products and orders
        var query = _context.OrderDetails
            .Include(od => od.Product)
            .Include(od => od.Order)
            .Where(od => od.Order.DateOfSale >= startDate && od.Order.DateOfSale <= endDate)
            .AsQueryable();

        // Filter by category if provided
        if (!string.IsNullOrEmpty(category))
            query = query.Where(od => od.Product.Category == category);

        // Filter by region if provided
        if (!string.IsNullOrEmpty(region))
            query = query.Where(od => od.Order.Region == region);

        // Group by Product and calculate total quantity sold
        var topProducts = await query
            .GroupBy(od => new
            {
                od.ProductId,
                od.Product.Name,
                od.Product.Category
            })
            .Select(g => new
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                Category = g.Key.Category,
                TotalQuantitySold = g.Sum(x => x.QuantitySold)
            })
            .OrderByDescending(x => x.TotalQuantitySold)
            .Take(topN)
            .ToListAsync();

        return Ok(topProducts);
    }

}