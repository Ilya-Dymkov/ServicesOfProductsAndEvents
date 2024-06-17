using Microsoft.AspNetCore.Mvc;
using ServicesOfProducts.Controllers.ControllersSource;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Proxies;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly ITransactionService _transactionService = new TransactionServiceProxy(dbContext);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAll() => 
        Ok(await _transactionService.GetAll());
    
    [HttpGet("{number}/{name}")]
    public async Task<ActionResult<Transaction>> Get(uint number, string name) => 
        await this.BaseActionGet(() => _transactionService.Get(number, name), 
            $"There is no transaction with order number '{number}' or product name '{name}'!");
}