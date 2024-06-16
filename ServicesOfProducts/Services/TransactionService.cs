using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Services;

public class TransactionService(ApplicationDbContext dbContext) : ITransactionService
{
    public Task<IEnumerable<Transaction>> GetAll() =>
        Task.FromResult(dbContext.Transactions.AsEnumerable());

    public Task<IEnumerable<Transaction>> GetByOrder(uint orderNumber) =>
        Task.FromResult(dbContext.Transactions
            .Where(t => t.Order.Number == orderNumber)
            .AsEnumerable());
    
    public Task<IEnumerable<Transaction>> GetByProduct(string productName) =>
        Task.FromResult(dbContext.Transactions
            .Where(t => t.Product.Name == productName)
            .AsEnumerable());

    public Task<Transaction?> Get(uint orderNumber, string productName) =>
        dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Order.Number == orderNumber && t.Product.Name == productName);

    private async Task<Transaction> CreateTransactionFromNumberAndName(uint orderNumber, string productName) =>
        Transaction.CreateInstance(
            await dbContext.Orders.FirstOrDefaultAsync(o => o.Number == orderNumber) ??
                throw new ArgumentException($"There is no order with number '{orderNumber}'!"),
            await dbContext.Products.FirstOrDefaultAsync(p => p.Name == productName) ??
                throw new ArgumentException($"There is no product with name '{productName}'!"));

    public async Task<Transaction?> Add(uint orderNumber, string productName)
    {
        await dbContext.Transactions.AddAsync(
            await CreateTransactionFromNumberAndName(orderNumber, productName));
        
        return await Get(orderNumber, productName);
    }

    public async Task<IEnumerable<Transaction>> AddMany(uint orderNumber, IEnumerable<string> products)
    {
        await dbContext.Transactions.AddRangeAsync(
            products.Select(async productName =>
                    await CreateTransactionFromNumberAndName(orderNumber, productName))
                .Select(t => t.Result));
        
        return await Task.FromResult(dbContext.Transactions.Where(t => t.Order.Number == orderNumber));
    }
    
    private async Task BaseChange(uint orderNumber, string productName, Action<Transaction> actionUpdate)
    {
        var transaction = await Get(orderNumber, productName);

        if (transaction == null)
            throw new ArgumentException(
                $"There is no transaction with order '{orderNumber}' or product '{productName}'!");

        actionUpdate(transaction);
    }

    public async Task Delete(uint orderNumber, string productName) =>
        await BaseChange(orderNumber, productName,
            transaction => dbContext.Transactions.Remove(transaction));

    public async Task DeleteByOrder(uint orderNumber)
    {
        if (await dbContext.Orders.FirstOrDefaultAsync(o => o.Number == orderNumber) == null)
            throw new ArgumentException($"There is no order with number '{orderNumber}'!");
        
        dbContext.Transactions.RemoveRange(
            dbContext.Transactions.Where(t => t.Order.Number == orderNumber));
    }
    
    public async Task DeleteByProduct(string productName)
    {
        if (await dbContext.Products.FirstOrDefaultAsync(p => p.Name == productName) == null)
            throw new ArgumentException($"There is no product with name '{productName}'!");

        dbContext.Transactions.RemoveRange(
            dbContext.Transactions.Where(t => t.Product.Name == productName));
    }
}