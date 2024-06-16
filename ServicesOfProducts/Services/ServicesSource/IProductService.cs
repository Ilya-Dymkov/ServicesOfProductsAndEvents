using ServicesOfProducts.Models;

namespace ServicesOfProducts.Services.ServicesSource;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product?> Get(string name);
    Task<IEnumerable<Transaction>> GetTransactions(string productName);
    Task<Product?> Add(string name, string categoryName, uint price, uint quantity, bool enable);
    Task<Product?> UpdateName(string oldName, string newName);
    Task<Product?> UpdateData(string name, string categoryName, uint cost, uint count, bool enable);
    Task Delete(string name);
}