using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.App.Backend.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Northwind.App.Backend.Controllers;

/// <summary>
/// API for managing products
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly NorthwindContext _db;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(NorthwindContext db, ILogger<ProductsController> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <param name="skip">Number of products to skip (default: 0)</param>
    /// <param name="take">Number of products to return (default: 1000)</param>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all products", Description = "Returns a paginated list of all products with category and supplier information")]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 1000)
    {
        _logger.LogInformation("Getting products with skip={Skip}, take={Take}", skip, take);

        var products = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Include(p => p.OrderDetails)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        _logger.LogInformation("Returning {Count} products", products.Count);

        return Ok(products);
    }
}
