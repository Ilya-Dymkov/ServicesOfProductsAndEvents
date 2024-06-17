using ServicesOfProducts.DataContext;
using ServicesOfProducts.Loggers;
using ServicesOfProducts.Loggers.LoggersSource;
using ServicesOfProducts.Models;
using ServicesOfProducts.Models.ModelsSource;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Proxies;

public class UserServiceProxy(ApplicationDbContext dbContext) : IUserService
{
    private readonly IUserService _userService = new UserService(dbContext);
    private readonly IProxyLogger _logger = ProxyLogger.CreateInstance<IUserService>();

    public Task<IEnumerable<User>> GetAll()
    {
        _logger.ToLogInfo("Get all users.");
        
        return _userService.GetAll();
    }

    public Task<User?> Get(string login)
    {
        _logger.ToLogInfo($"Get user with login '{login}'.");

        return _userService.Get(login);
    }

    public Task<IEnumerable<Order>> GetOrders(string login)
    {
        _logger.ToLogInfo($"Get orders with user login '{login}'.");

        return _userService.GetOrders(login);
    }

    public Task<IEnumerable<Transaction>> GetTransactions(string login)
    {
        _logger.ToLogInfo($"Get transactions with user login '{login}'.");

        return _userService.GetTransactions(login);
    }

    public Task<User?> Add(string login, string name, Genders gender, DateTime birthday, bool isAdmin)
    {
        _logger.ToLogInfo($"Add new user with login '{login}', name '{name}', gender '{gender}', " +
                  $"birthday '{birthday}', is admin '{isAdmin}'.");

        return _userService.Add(login, name, gender, birthday, isAdmin);
    }

    public Task<User?> UpdateLogin(string oldLogin, string newLogin)
    {
        _logger.ToLogInfo($"Update user login from '{oldLogin}' to '{newLogin}'.");

        return _userService.UpdateLogin(oldLogin, newLogin);
    }

    public Task<User?> UpdateData(string login, string name, Genders gender, DateTime birthday, bool isAdmin)
    {
        _logger.ToLogInfo($"Update data of user with login '{login}' to name '{name}', gender '{gender}', " +
                  $"birthday '{birthday}', is admin '{isAdmin}'.");

        return _userService.UpdateData(login, name, gender, birthday, isAdmin);
    }

    public Task<User?> Recovery(string login)
    {
        _logger.ToLogInfo($"Recovery user with login '{login}'.");

        return _userService.Recovery(login);
    }

    public Task SoftDelete(string login)
    {
        _logger.ToLogInfo($"Soft delete user with login '{login}'.");

        return _userService.SoftDelete(login);
    }

    public Task HardDelete(string login)
    {
        _logger.ToLogInfo($"Hard delete user with login '{login}'.");

        return _userService.HardDelete(login);
    }
}