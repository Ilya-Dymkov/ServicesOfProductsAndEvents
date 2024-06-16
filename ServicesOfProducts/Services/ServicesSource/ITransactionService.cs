using ServicesOfProducts.Models;

namespace ServicesOfProducts.Services.ServicesSource;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAll();
    Task<IEnumerable<Transaction>> GetByOrder(uint orderNumber);
    Task<IEnumerable<Transaction>> GetByProduct(string productName);
    Task<Transaction?> Get(uint orderNumber, string productName);
    Task<Transaction?> Add(uint orderNumber, string productName);
    Task<IEnumerable<Transaction>> AddMany(uint orderNumber, IEnumerable<string> productNames);
    Task Delete(uint orderNumber, string productName);
    Task DeleteByOrder(uint orderNumber);
    Task DeleteByProduct(string productName);
}