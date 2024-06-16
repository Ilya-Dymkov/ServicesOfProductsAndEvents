using ServicesOfProducts.Models;

namespace ServicesOfProducts.Services.ServicesSource;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAll();
    Task<Category?> Get(string name);
    Task<Category?> Add(string name);
    Task<Category?> UpdateName(string oldName, string newName);
    Task Delete(string name);
}