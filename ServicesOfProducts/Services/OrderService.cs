using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Models.ModelsSource;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Services;

public class OrderService(ApplicationDbContext dbContext) : IOrderService
{
    public Task<IEnumerable<Order>> GetAll() => 
        Task.FromResult(dbContext.Orders.AsEnumerable());

    public Task<Order?> Get(uint number) => 
        dbContext.Orders.FirstOrDefaultAsync(o => o.Number == number);

    public Task<IEnumerable<Transaction>> GetTransactions(uint orderNumber) =>
        Task.FromResult(dbContext.Transactions
            .Where(t => t.Order.Number == orderNumber)
            .AsEnumerable());

    private async Task<uint> CheckNumber(uint number)
    {
        if (await dbContext.Orders.FirstOrDefaultAsync(o => o.Number == number) != null)
            throw new ArgumentException($"Order with {number} number already exists!");
        
        return number;
    }

    private uint CheckProductQuantity(Product product, uint checkQuantity)
    {
        if (product.Quantity < checkQuantity)
            throw new ArgumentException($"The requested quantity '{checkQuantity}' is too much!");

        if (product.Quantity == checkQuantity)
            product.Enable = false;

        product.Quantity -= checkQuantity;
        return checkQuantity;
    }
    
    private async Task<Transaction> CreateTransaction(Order order, InputProductInfo productInfo)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Name == productInfo.Name) ??
                      throw new ArgumentException($"There is no product with name '{productInfo.Name}'!");
        
        return await Task.FromResult(Transaction.CreateInstance(order, product,
            CheckProductQuantity(product, productInfo.Quantity), productInfo.Discount));
    }

    private async Task<Order?> BaseAdd(uint number, string userLogin, Func<Order, Task> addTransaction)
    {
        var order = Order.CreateInstance(await CheckNumber(number),
            await dbContext.Users.FirstOrDefaultAsync(u => u.Login == userLogin) ??
            throw new ArgumentException($"There is no user with login '{userLogin}'!"));
        
        await dbContext.Orders.AddAsync(order);
        await addTransaction(order);
        
        await dbContext.SaveChangesAsync();
        return await Get(number);
    }
    
    private async Task AddTransaction(Order order, InputProductInfo product) =>
        await dbContext.Transactions.AddAsync(
            await CreateTransaction(order, product));

    public async Task<Order?> Add(uint number, string userLogin, InputProductInfo product) => 
        await BaseAdd(number, userLogin, order => AddTransaction(order, product));
    
    private async Task AddTransactions(Order order, IEnumerable<InputProductInfo> products) =>
        await dbContext.Transactions.AddRangeAsync(
            products.Select(async product => await CreateTransaction(order, product))
                .Select(t => t.Result));

    public async Task<Order?> AddMany(uint number, string userLogin, IEnumerable<InputProductInfo> products) => 
        await BaseAdd(number, userLogin, order => AddTransactions(order, products));

    private async Task BaseChange(uint number, Action<Order> actionUpdate)
    {
        var order = await Get(number);

        if (order == null)
            throw new ArgumentException($"There is no order with number '{number}'!");

        actionUpdate(order);
        
        await dbContext.SaveChangesAsync();
    }
    
    public async Task SoftDelete(uint number) =>
        await BaseChange(number, order => order.IsDeleted = true);

    public async Task HardDelete(uint number) =>
        await BaseChange(number, order =>
        {
            dbContext.Transactions.RemoveRange(
                dbContext.Transactions.Where(t => t.Order.Number == number));
            
            dbContext.Orders.Remove(order);
        });
}