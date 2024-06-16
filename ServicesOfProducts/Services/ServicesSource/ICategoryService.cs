using ServicesOfProducts.Models;

namespace ServicesOfProducts.Services.ServicesSource;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAll();
    Task<IEnumerable<Category>> GetSubsidiary(string parentName);
    Task<Category?> Get(string name);
    Task<IEnumerable<Product>> GetProducts(string categoryName);
    Task<Category?> Add(string name, string? parentName);
    Task<Category?> UpdateName(string oldName, string newName);
    Task<Category?> UpdateParent(string name, string parentName);
    Task Delete(string name);
}