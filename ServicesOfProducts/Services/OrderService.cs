using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Services;

public class OrderService(ApplicationDbContext dbContext) : IOrderService
{
    public Task<IEnumerable<Order>> GetAll() => 
        Task.FromResult(dbContext.Orders.AsEnumerable());

    public Task<Order?> Get(uint number) => 
        dbContext.Orders.FirstOrDefaultAsync(o => o.Number == number);

    public async Task<Order?> Add(uint number)
    {
        await dbContext.Orders.AddAsync(Order.CreateInstance(number));
        await dbContext.SaveChangesAsync();
        return await Get(number);
    }
    
    private async Task BaseChange(uint number, Action<Order> actionUpdate)
    {
        var order = await Get(number);

        if (order == null)
            throw new ArgumentException($"There is no order with number '{number}'!");

        actionUpdate(order);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(uint number) => 
        await BaseChange(number, order => dbContext.Orders.Remove(order));
}