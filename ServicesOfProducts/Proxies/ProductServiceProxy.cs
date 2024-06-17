using ServicesOfProducts.DataContext;
using ServicesOfProducts.Loggers;
using ServicesOfProducts.Loggers.LoggersSource;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Proxies;

public class ProductServiceProxy(ApplicationDbContext dbContext) : IProductService
{
    private readonly IProductService _productService = new ProductService(dbContext);
    private readonly IProxyLogger _logger = ProxyLogger.CreateInstance<IProductService>();
    
    public Task<IEnumerable<Product>> GetAll()
    {
        _logger.ToLogInfo("Get all products.");

        return _productService.GetAll();
    }

    public Task<Product?> Get(string name)
    {
        _logger.ToLogInfo($"Get product with name '{name}'.");

        return _productService.Get(name);
    }

    public Task<IEnumerable<Transaction>> GetTransactions(string productName)
    {
        _logger.ToLogInfo($"Get transactions with product name '{productName}'.");

        return _productService.GetTransactions(productName);
    }

    public Task<Product?> Add(string name, string categoryName, uint price, uint quantity, bool enable)
    {
        _logger.ToLogInfo($"Add new product with name '{name}', category name '{categoryName}', " +
                          $"price '{price}', quantity '{quantity}', enable '{enable}'.");

        return _productService.Add(name, categoryName, price, quantity, enable);
    }

    public Task<Product?> UpdateName(string oldName, string newName)
    {
        _logger.ToLogInfo($"Update product name from '{oldName}' to '{newName}'.");

        return _productService.UpdateName(oldName, newName);
    }

    public Task<Product?> UpdateData(string name, string categoryName, uint price, uint quantity, bool enable)
    {
        _logger.ToLogInfo($"Update data of product with name '{name}' to category name '{categoryName}', " +
                          $"price '{price}', quantity '{quantity}', enable '{enable}'.");

        return _productService.UpdateData(name, categoryName, price, quantity, enable);
    }

    public Task Delete(string name)
    {
        _logger.ToLogInfo($"Delete product with name '{name}'.");

        return _productService.Delete(name);
    }
}