using ServicesOfProducts.Models;

namespace ServicesOfProducts.Services.ServicesSource;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAll();
    Task<Order?> Get(uint number);
    Task<Order?> Add(uint number);
    Task Delete(uint number);
}