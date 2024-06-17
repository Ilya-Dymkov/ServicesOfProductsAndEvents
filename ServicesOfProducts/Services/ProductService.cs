using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Services;

public class ProductService(ApplicationDbContext dbContext) : IProductService
{
    public Task<IEnumerable<Product>> GetAll() =>
        Task.FromResult(dbContext.Products.AsEnumerable());

    public Task<Product?> Get(string name) => 
        dbContext.Products.FirstOrDefaultAsync(p => p.Name == name);

    public Task<IEnumerable<Transaction>> GetTransactions(string productName) =>
        Task.FromResult(dbContext.Transactions
            .Where(t => t.Product.Name == productName)
            .AsEnumerable());

    private async Task<string> CheckName(string name)
    {
        if (await dbContext.Products.FirstOrDefaultAsync(p => p.Name == name) != null)
            throw new ArgumentException($"Product with {name} name already exists!");
        
        return name;
    }
    
    public async Task<Product?> Add(string name, string categoryName, uint price, uint quantity, bool enable)
    {
        await dbContext.Products.AddAsync(Product.CreateInstance(await CheckName(name),
            await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName), price, quantity, enable));
        await dbContext.SaveChangesAsync();
        return await Get(name);
    }

    private async Task BaseChange(string name, Action<Product> actionUpdate)
    {
        var product = await Get(name);

        if (product == null)
            throw new ArgumentException($"There is no product with name '{name}'!");

        actionUpdate(product);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task<Product?> UpdateName(string oldName, string newName)
    {
        await BaseChange(oldName, product => product.Name = CheckName(newName).Result);
        return await Get(newName);
    }
    
    public async Task<Product?> UpdateData(string name, string categoryName, uint price, uint quantity, bool enable)
    {
        await BaseChange(name, product =>
        {
            product.Category = dbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName).Result;
            product.Price = price;
            product.Quantity = quantity;
            product.Enable = enable;
        });
        return await Get(name);
    }
    
    private async Task DeleteTransactions(string productName)
    {
        if (await dbContext.Products.FirstOrDefaultAsync(p => p.Name == productName) == null)
            throw new ArgumentException($"There is no product with name '{productName}'!");

        dbContext.Transactions.RemoveRange(
            dbContext.Transactions.Where(t => t.Product.Name == productName));
    }

    public async Task Delete(string name) => 
        await BaseChange(name, product =>
        {
            DeleteTransactions(name).Wait();
            dbContext.Remove(product);
        });
}