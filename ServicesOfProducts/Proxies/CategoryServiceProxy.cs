using ServicesOfProducts.DataContext;
using ServicesOfProducts.Loggers;
using ServicesOfProducts.Loggers.LoggersSource;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Proxies;

public class CategoryServiceProxy(ApplicationDbContext dbContext) : ICategoryService
{
    private readonly ICategoryService _categoryService = new CategoryService(dbContext);
    private readonly IProxyLogger _logger = ProxyLogger.CreateInstance<ICategoryService>();
    
    public Task<IEnumerable<Category>> GetAll()
    {
        _logger.ToLogInfo("Get all categories.");

        return _categoryService.GetAll();
    }

    public Task<IEnumerable<Category>> GetSubsidiary(string parentName)
    {
        _logger.ToLogInfo($"Get subsidiary categories for category with name '{parentName}'.");
    
        return _categoryService.GetSubsidiary(parentName);
    }

    public Task<Category?> Get(string name)
    {
        _logger.ToLogInfo($"Get category with name '{name}'.");

        return _categoryService.Get(name);
    }

    public Task<IEnumerable<Product>> GetProducts(string categoryName)
    {
        _logger.ToLogInfo($"Get products with category name '{categoryName}'.");

        return _categoryService.GetProducts(categoryName);
    }

    public Task<Category?> Add(string name, string? parentName)
    {
        _logger.ToLogInfo($"Add new category with name '{name}'" +
                          (parentName == null ? " without parent category" : $" with parent category '{parentName}'") +
                          '.');

        return _categoryService.Add(name, parentName);
    }

    public Task<Category?> UpdateName(string oldName, string newName)
    {
        _logger.ToLogInfo($"Update category name from '{oldName}' to '{newName}'.");

        return _categoryService.UpdateName(oldName, newName);
    }

    public Task<Category?> UpdateParent(string name, string parentName)
    {
        _logger.ToLogInfo($"Update parent with name '{parentName}' for category with name '{name}'.");
    
        return _categoryService.UpdateParent(name, parentName);
    }

    public Task Delete(string name)
    {
        _logger.ToLogInfo($"Delete category with name '{name}'.");

        return _categoryService.Delete(name);
    }
}