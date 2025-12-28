using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.App.Backend.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Northwind.App.Backend.Controllers;

/// <summary>
/// Public API for managing customers. No authentication required.
/// </summary>
[ApiController]
[Route("api/public/customers")]
[Produces("application/json")]
[Tags("Public Customers")]
public class PublicCustomersController : ControllerBase
{
    private readonly NorthwindContext _db;
    private readonly ILogger<PublicCustomersController> _logger;

    public PublicCustomersController(NorthwindContext db, ILogger<PublicCustomersController> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Get all customers
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all customers", Description = "Returns a list of all customers")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
    {
        _logger.LogInformation("Getting all customers");

        var customers = await _db.Customers
            .AsNoTracking()
            .ToListAsync();

        return Ok(customers);
    }

    /// <summary>
    /// Get a customer by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get customer by ID", Description = "Returns a single customer by their ID")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        _logger.LogInformation("Getting customer {CustomerId}", id);

        var customer = await _db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
        {
            _logger.LogWarning("Customer {CustomerId} not found", id);
            return NotFound(new ProblemDetails
            {
                Title = "Customer not found",
                Detail = $"Customer with ID {id} was not found",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(customer);
    }

    /// <summary>
    /// Get a customer by ID including their orders
    /// </summary>
    [HttpGet("{id:int}/orders")]
    [SwaggerOperation(Summary = "Get customer with orders", Description = "Returns a customer with their most recent orders (limited)")]
    [ProducesResponseType(typeof(CustomerWithOrdersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerWithOrdersResponse>> GetByIdWithOrders(int id, [FromQuery] int maxOrders = 10)
    {
        _logger.LogInformation("Getting customer {CustomerId} with max {MaxOrders} orders", id, maxOrders);

        var customer = await _db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
        {
            _logger.LogWarning("Customer {CustomerId} not found", id);
            return NotFound(new ProblemDetails
            {
                Title = "Customer not found",
                Detail = $"Customer with ID {id} was not found",
                Status = StatusCodes.Status404NotFound
            });
        }

        // Get orders separately with limit
        var orders = await _db.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == id)
            .OrderByDescending(o => o.OrderDate)
            .Take(maxOrders)
            .Include(o => o.Employee)
            .Include(o => o.Shipper)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product!)
                    .ThenInclude(p => p.Supplier)
            .ToListAsync();

        var totalOrderCount = await _db.Orders.CountAsync(o => o.CustomerId == id);

        return Ok(new CustomerWithOrdersResponse
        {
            Customer = customer,
            Orders = orders,
            TotalOrderCount = totalOrderCount,
            ReturnedOrderCount = orders.Count
        });
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new customer", Description = "Creates a new customer and returns the created resource")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Customer>> Create([FromBody] Customer customer)
    {
        _logger.LogInformation("Creating new customer: {CustomerName}", customer.CustomerName);

        // Ensure ID is not set by client
        customer.CustomerId = 0;
        customer.Orders = new List<Order>();

        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Created customer {CustomerId}", customer.CustomerId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.CustomerId },
            customer);
    }

    /// <summary>
    /// Update a customer (full replacement)
    /// </summary>
    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Update a customer", Description = "Replaces all properties of an existing customer")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Customer>> Update(int id, [FromBody] Customer input)
    {
        _logger.LogInformation("Updating customer {CustomerId}", id);

        var customer = await _db.Customers.FindAsync(id);

        if (customer == null)
        {
            _logger.LogWarning("Customer {CustomerId} not found for update", id);
            return NotFound(new ProblemDetails
            {
                Title = "Customer not found",
                Detail = $"Customer with ID {id} was not found",
                Status = StatusCodes.Status404NotFound
            });
        }

        customer.CustomerName = input.CustomerName;
        customer.ContactName = input.ContactName;
        customer.Address = input.Address;
        customer.City = input.City;
        customer.PostalCode = input.PostalCode;
        customer.Country = input.Country;

        await _db.SaveChangesAsync();

        _logger.LogInformation("Updated customer {CustomerId}", id);

        return Ok(customer);
    }

    /// <summary>
    /// Partially update a customer
    /// </summary>
    [HttpPatch("{id:int}")]
    [SwaggerOperation(Summary = "Partially update a customer", Description = "Updates only the provided properties of an existing customer")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Customer>> Patch(int id, [FromBody] Customer input)
    {
        _logger.LogInformation("Patching customer {CustomerId}", id);

        var customer = await _db.Customers.FindAsync(id);

        if (customer == null)
        {
            _logger.LogWarning("Customer {CustomerId} not found for patch", id);
            return NotFound(new ProblemDetails
            {
                Title = "Customer not found",
                Detail = $"Customer with ID {id} was not found",
                Status = StatusCodes.Status404NotFound
            });
        }

        // Only update properties that are provided (not null)
        if (input.CustomerName != null) customer.CustomerName = input.CustomerName;
        if (input.ContactName != null) customer.ContactName = input.ContactName;
        if (input.Address != null) customer.Address = input.Address;
        if (input.City != null) customer.City = input.City;
        if (input.PostalCode != null) customer.PostalCode = input.PostalCode;
        if (input.Country != null) customer.Country = input.Country;

        await _db.SaveChangesAsync();

        _logger.LogInformation("Patched customer {CustomerId}", id);

        return Ok(customer);
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Delete a customer", Description = "Permanently deletes a customer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting customer {CustomerId}", id);

        var customer = await _db.Customers.FindAsync(id);

        if (customer == null)
        {
            _logger.LogWarning("Customer {CustomerId} not found for deletion", id);
            return NotFound(new ProblemDetails
            {
                Title = "Customer not found",
                Detail = $"Customer with ID {id} was not found",
                Status = StatusCodes.Status404NotFound
            });
        }

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Deleted customer {CustomerId}", id);

        return NoContent();
    }
}
