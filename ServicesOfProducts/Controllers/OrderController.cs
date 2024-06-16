using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
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

    private async Task<ActionResult<Order>> BaseActionGet(Func<Task<Order?>> func, string errorMessage)
    {
        var category = await func();

        if (category == null) throw new Exception(errorMessage);

        return Ok(category);
    }

    [HttpGet("{number}")]
    public async Task<ActionResult<Order>> Get(uint number) => 
        await BaseActionGet(() => _orderService.Get(number), 
            $"There is no order with number '{number}'!");

    [HttpPost("{number}")]
    public async Task<ActionResult<Order>> Add(uint number) =>
        await BaseActionGet(() => _orderService.Add(number),
            $"The new order with number '{number}' could not be created!");

    [HttpDelete("{number}")]
    public async Task<IActionResult> Delete(uint number)
    {
        await _orderService.Delete(number);
        return Ok($"The order with number '{number}' has been deleted!");
    }
}