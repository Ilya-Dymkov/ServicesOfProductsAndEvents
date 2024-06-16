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

    public async Task<Product?> Add(string name, string categoryName, uint cost, uint count, bool enable)
    {
        await dbContext.Products.AddAsync(Product.CreateInstance(name,
            await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName), cost, count, enable));
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
        await BaseChange(oldName, product => product.Name = newName);
        return await Get(newName);
    }
    
    public async Task<Product?> UpdateData(string name, string categoryName, uint cost, uint count, bool enable)
    {
        await BaseChange(name, product =>
        {
            product.Category = dbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName).Result;
            product.Cost = cost;
            product.Enable = enable;
        });
        return await Get(name);
    }

    public async Task Delete(string name) => 
        await BaseChange(name, product => dbContext.Remove(product));
}