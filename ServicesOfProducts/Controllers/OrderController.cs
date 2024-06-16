using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.Controllers.ControllersSource;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Models.ModelsSource;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly IOrderService _orderService = new OrderService(dbContext);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll() => 
        Ok(await _orderService.GetAll());

    [HttpGet("{number}")]
    public async Task<ActionResult<Order>> Get(uint number) => 
        await this.BaseActionGet(() => _orderService.Get(number), 
            $"There is no order with number '{number}'!");

    [HttpGet("{number}/transactions")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(uint number) =>
        await this.BaseActionGet(() => _orderService.GetTransactions(number)!,
            $"There is no order with number '{number}'!");

    [HttpPost("{user}/{number}")]
    public async Task<ActionResult<Order>> Add(string user, uint number, InputProductInfo product) =>
        await this.BaseActionGet(() => _orderService.Add(number, user, product),
            $"The new order with number '{number}' could not be created!");
    
    [HttpPost("{user}/{number}/many")]
    public async Task<ActionResult<Order>> AddMany(string user, uint number, IEnumerable<InputProductInfo> products) =>
        await this.BaseActionGet(() => _orderService.AddMany(number, user, products),
            $"The new order with number '{number}' could not be created!");

    [HttpDelete("{number}")]
    public async Task<IActionResult> SoftDelete(uint number)
    {
        await _orderService.SoftDelete(number);
        return Ok($"The order with number '{number}' has been deleted!");
    }
    
    [HttpDelete("{number}/hard")]
    public async Task<IActionResult> HardDelete(uint number)
    {
        await _orderService.HardDelete(number);
        return Ok($"The order with number '{number}' has been full deleted!");
    }
}