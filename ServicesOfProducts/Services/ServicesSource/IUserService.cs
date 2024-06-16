using ServicesOfProducts.Models;
using ServicesOfProducts.Models.ModelsSource;

namespace ServicesOfProducts.Services.ServicesSource;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User?> Get(string login);
    Task<IEnumerable<Order>> GetOrders(string login);
    Task<IEnumerable<Transaction>> GetTransactions(string login);
    Task<User?> Add(string login, string name, Genders gender, DateTime birthday, bool isAdmin);
    Task<User?> UpdateLogin(string oldLogin, string newLogin);
    Task<User?> UpdateData(string login, string name, Genders gender, DateTime birthday, bool isAdmin);
    Task<User?> Recovery(string login);
    Task SoftDelete(string login);
    Task HardDelete(string login);
}