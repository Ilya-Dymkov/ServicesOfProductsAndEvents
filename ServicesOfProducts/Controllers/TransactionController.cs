using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly ITransactionService _transactionService = new TransactionService(dbContext);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAll() => 
        Ok(await _transactionService.GetAll());

    private async Task<ActionResult<TResult>> BaseActionGet<TResult>(Func<Task<TResult>> func, string errorMessage)
    {
        var category = await func();
    
        if (category == null) throw new Exception(errorMessage);
    
        return Ok(category);
    }

    [HttpGet("{number}/{name}")]
    public async Task<ActionResult<Transaction?>> Get(uint number, string name) => 
        await BaseActionGet(() => _transactionService.Get(number, name), 
            $"There is no transaction with order '{number}' or product '{name}'!");
    
    [HttpGet("byOrder/{number}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetByOrder(uint number) => 
        await BaseActionGet(() => _transactionService.GetByOrder(number), 
            $"There is no transaction with order '{number}'!");
    
    [HttpGet("byProduct/{name}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetByProduct(string name) => 
        await BaseActionGet(() => _transactionService.GetByProduct(name), 
            $"There is no transaction with product '{name}'!");

    [HttpPost("{number}/{name}")]
    public async Task<ActionResult<Transaction?>> Add(uint number, string name) =>
        await BaseActionGet(() => _transactionService.Add(number, name),
            $"The new transaction with order '{number}' and product '{name}' could not be created!");
    
    [HttpPost("{number}/products")]
    public async Task<ActionResult<IEnumerable<Transaction>>> AddMany(uint number, IEnumerable<string> names) =>
        await BaseActionGet(() => _transactionService.AddMany(number, names),
            $"The new transactions with order '{number}' could not be created!");

    [HttpDelete("{number}/{name}")]
    public async Task<IActionResult> Delete(uint number, string name)
    {
        await _transactionService.Delete(number, name);
        return Ok($"The transaction with order '{number}' and product '{name}' has been deleted!");
    }
    
    [HttpDelete("byOrder/{number}")]
    public async Task<IActionResult> DeleteByOrder(uint number)
    {
        await _transactionService.DeleteByOrder(number);
        return Ok($"The transactions with order '{number}' has been deleted!");
    }
    
    [HttpDelete("byProduct/{name}")]
    public async Task<IActionResult> DeleteByProduct(string name)
    {
        await _transactionService.DeleteByProduct(name);
        return Ok($"The transaction with product '{name}' has been deleted!");
    }
}