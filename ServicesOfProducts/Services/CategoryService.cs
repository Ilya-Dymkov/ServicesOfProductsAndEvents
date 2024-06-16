using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Services;

public class CategoryService(ApplicationDbContext dbContext) : ICategoryService
{
    public Task<IEnumerable<Category>> GetAll() => 
        Task.FromResult(dbContext.Categories.AsEnumerable());

    public Task<IEnumerable<Category>> GetSubsidiary(string parentName) =>
        Task.FromResult(dbContext.Categories
            .Where(c => (c.ParentCategory != null ? c.ParentCategory.Name : null) == parentName)
            .AsEnumerable());

    public Task<Category?> Get(string name) => 
        dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);

    public Task<IEnumerable<Product>> GetProducts(string categoryName) =>
        Task.FromResult(dbContext.Products
            .Where(p => p.Category != null && p.Category.Name == categoryName)
            .AsEnumerable());
    
    private string CheckName(string name)
    {
        if (dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name) != null)
            throw new ArgumentException($"Category with {name} name already exists!");
        
        return name;
    }

    private async Task<Category> GetParentCategory(string parentName) =>
        await dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == parentName) ??
        throw new ArgumentException(
            $"There is no category with parent name '{parentName}'!");

    public async Task<Category?> Add(string name, string? parentName)
    {
        Category? parentCategory = null;
        if (parentName != null) parentCategory = await GetParentCategory(parentName);
        
        await dbContext.Categories.AddAsync(Category.CreateInstance(CheckName(name), parentCategory));
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
        await BaseChange(oldName, category => category.Name = CheckName(newName));
        return await Get(newName);
    }
    
    public async Task<Category?> UpdateParent(string name, string parentName)
    {
        await BaseChange(name, category => category.ParentCategory = GetParentCategory(parentName).Result);
        return await Get(name);
    }

    private Task RemoveCategoryForProducts(string categoryName)
    {
        foreach (var product in dbContext.Products.Where(p => 
                     p.Category != null && p.Category.Name == categoryName))
            product.Category = null;
        
        return Task.CompletedTask;
    }

    private Task RemoveParentCategory(string parentName)
    {
        foreach (var subCategory in dbContext.Categories.Where(c => 
                     c.ParentCategory != null && c.ParentCategory.Name == parentName)) 
            subCategory.ParentCategory = null;
        
        return Task.CompletedTask;
    }

    public async Task Delete(string name)
    {
        await BaseChange(name, ActionUpdate);
        return;

        async void ActionUpdate(Category category)
        {
            await RemoveCategoryForProducts(name);

            await RemoveParentCategory(name);

            dbContext.Categories.Remove(category);
        }
    }
}