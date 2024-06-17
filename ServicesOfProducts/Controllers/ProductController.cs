using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.Controllers.ControllersSource;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Proxies;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly IProductService _productService = new ProductServiceProxy(dbContext);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll() => 
        Ok(await _productService.GetAll());

    [HttpGet("{name}")]
    public async Task<ActionResult<Product>> Get(string name) => 
        await this.BaseActionGet(() => _productService.Get(name), 
            $"There is no product with name '{name}'!");
    
    [HttpGet("{name}/transactions")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(string name) => 
        await this.BaseActionGet(() => _productService.GetTransactions(name)!, 
            $"There is no product with name '{name}'!");

    [HttpPost("{name}")]
    public async Task<ActionResult<Product>> Add(string name, string categoryName,
        uint price, uint quantity, bool enable) =>
        await this.BaseActionGet(() => _productService.Add(name, categoryName, price, quantity, enable),
            $"The new product with name '{name}' could not be created!");

    [HttpPatch("{oldName}")]
    public async Task<ActionResult<Product>> UpdateName(string oldName, string newName) =>
        await this.BaseActionGet(() => _productService.UpdateName(oldName, newName),
            $"The product with name '{oldName}' to new name '{newName}' could not be updated!");

    [HttpPatch("{name}/data")]
    public async Task<ActionResult<Product>> UpdateData(string name, string categoryName,
        uint price, uint quantity, bool enable) =>
        await this.BaseActionGet(() => _productService.UpdateData(name, categoryName, price, quantity, enable),
            $"The data of product with name '{name}' could not be updated!");

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        await _productService.Delete(name);
        return Ok($"The product with name '{name}' has been deleted!");
    }
}