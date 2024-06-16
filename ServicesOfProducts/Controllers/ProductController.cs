using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly IProductService _productService = new ProductService(dbContext);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll() => 
        Ok(await _productService.GetAll());

    private async Task<ActionResult<Product>> BaseActionGet(Func<Task<Product?>> func, string errorMessage)
    {
        var product = await func();

        if (product == null) throw new Exception(errorMessage);

        return Ok(product);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Product>> Get(string name) => 
        await BaseActionGet(() => _productService.Get(name), 
            $"There is no product with name '{name}'!");

    [HttpPost("{name}")]
    public async Task<ActionResult<Product>> Add(string name, string categoryName,
        uint cost, uint count, bool enable) =>
        await BaseActionGet(() => _productService.Add(name, categoryName, cost, count, enable),
            $"The new product with name '{name}' could not be created!");

    [HttpPatch("{oldName}")]
    public async Task<ActionResult<Product>> UpdateName(string oldName, string newName) =>
        await BaseActionGet(() => _productService.UpdateName(oldName, newName),
            $"The product with name '{oldName}' to new name '{newName}' could not be updated!");

    [HttpPatch("{name}/data")]
    public async Task<ActionResult<Product>> UpdateData(string name, string categoryName,
        uint cost, uint count, bool enable) =>
        await BaseActionGet(() => _productService.UpdateData(name, categoryName, cost, count, enable),
            $"The data of product with name '{name}' could not be updated!");

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        await _productService.Delete(name);
        return Ok($"The product with name '{name}' has been deleted!");
    }
}