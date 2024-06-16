using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Services;

public class TransactionService(ApplicationDbContext dbContext) : ITransactionService
{
    public Task<IEnumerable<Transaction>> GetAll() =>
        Task.FromResult(dbContext.Transactions.AsEnumerable());

    public Task<Transaction?> Get(uint orderNumber, string productName) =>
        dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Order.Number == orderNumber && t.Product.Name == productName);
}