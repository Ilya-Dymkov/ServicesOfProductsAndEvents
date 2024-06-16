using ServicesOfProducts.Models;

namespace ServicesOfProducts.Services.ServicesSource;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAll();
    Task<Transaction?> Get(uint orderNumber, string productName);
}