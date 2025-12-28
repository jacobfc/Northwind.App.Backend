using Northwind.App.Backend.Models;

namespace Northwind.App.Backend.Controllers;

/// <summary>
/// Response for customer with orders endpoint
/// </summary>
public class CustomerWithOrdersResponse
{
    public Customer Customer { get; set; } = null!;
    public List<Order> Orders { get; set; } = new();
    public int TotalOrderCount { get; set; }
    public int ReturnedOrderCount { get; set; }
}
