using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Models.ModelsSource;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Services;

public class UserService(ApplicationDbContext dbContext) : IUserService
{
    public Task<IEnumerable<User>> GetAll() =>
        Task.FromResult(dbContext.Users.AsEnumerable());

    public Task<User?> Get(string login) =>
        dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);

    public Task<IEnumerable<Order>> GetOrders(string login) =>
        Task.FromResult(dbContext.Orders
            .Where(o => o.User.Login == login)
            .AsEnumerable());

    public Task<IEnumerable<Transaction>> GetTransactions(string login) =>
        Task.FromResult(dbContext.Transactions
            .Where(t => t.Order.User.Login == login)
            .AsEnumerable());
    
    private string CheckLogin(string login)
    {
        if (dbContext.Users.FirstOrDefaultAsync(u => u.Login == login) != null)
            throw new ArgumentException($"User with {login} login already exists!");
        
        return login;
    }

    public async Task<User?> Add(string login, string name, Genders gender, DateTime birthday, bool isAdmin)
    {
        await dbContext.Users.AddAsync(User.CreateInstance(CheckLogin(login), name, gender, birthday, isAdmin));
        await dbContext.SaveChangesAsync();
        return await Get(login);
    }
    
    private async Task BaseChange(string login, Action<User> actionUpdate)
    {
        var user = await Get(login);

        if (user == null)
            throw new ArgumentException($"There is no user with login '{login}'!");

        actionUpdate(user);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task<User?> UpdateLogin(string oldLogin, string newLogin)
    {
        await BaseChange(oldLogin, user => user.Login = newLogin);
        return await Get(newLogin);
    }

    public async Task<User?> UpdateData(string login, string name, Genders gender, DateTime birthday, bool isAdmin)
    {
        await BaseChange(login, user =>
        {
            user.Name = name;
            user.Gender = gender;
            user.Birthday = birthday;
            user.IsAdmin = isAdmin;
        });
        return await Get(login);
    }

    public async Task<User?> Recovery(string login)
    {
        await BaseChange(login, user => user.IsDeleted = false);
        return await Get(login);
    }

    public async Task SoftDelete(string login) =>
        await BaseChange(login, user => user.IsDeleted = true);
    
    public async Task HardDelete(string login) =>
        await BaseChange(login, user =>
        {
            dbContext.Transactions.RemoveRange(
                dbContext.Transactions.Where(t => t.Order.User.Login == login));
            
            dbContext.Orders.RemoveRange(
                dbContext.Orders.Where(o => o.User.Login == login));

            dbContext.Users.Remove(user);
        });
}