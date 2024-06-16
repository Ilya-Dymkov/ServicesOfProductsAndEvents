using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Services;

public class CategoryService(ApplicationDbContext dbContext) : ICategoryService
{
    public Task<IEnumerable<Category>> GetAll() => 
        Task.FromResult(dbContext.Categories.AsEnumerable());

    public Task<Category?> Get(string name) => 
        dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);

    public async Task<Category?> Add(string name)
    {
        await dbContext.Categories.AddAsync(Category.CreateInstance(name));
        await dbContext.SaveChangesAsync();
        return await Get(name);
    }
    
    private async Task BaseChange(string name, Action<Category> actionUpdate)
    {
        var category = await Get(name);

        if (category == null)
            throw new ArgumentException($"There is no category with name '{name}'!");

        actionUpdate(category);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task<Category?> UpdateName(string oldName, string newName)
    {
        await BaseChange(oldName, category => category.Name = newName);
        return await Get(newName);
    }

    public async Task Delete(string name) => 
        await BaseChange(name, category => dbContext.Categories.Remove(category));
}