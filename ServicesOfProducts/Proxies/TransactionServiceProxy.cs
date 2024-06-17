using ServicesOfProducts.DataContext;
using ServicesOfProducts.Loggers;
using ServicesOfProducts.Loggers.LoggersSource;
using ServicesOfProducts.Models;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Proxies;

public class TransactionServiceProxy(ApplicationDbContext dbContext) : ITransactionService
{
    private readonly ITransactionService _transactionService = new TransactionService(dbContext);
    private readonly IProxyLogger _logger = ProxyLogger.CreateInstance<ITransactionService>();
    
    public Task<IEnumerable<Transaction>> GetAll()
    {
        _logger.ToLogInfo("Get all transactions.");

        return _transactionService.GetAll();
    }

    public Task<Transaction?> Get(uint orderNumber, string productName)
    {
        _logger.ToLogInfo($"Get transaction with order number '{orderNumber}' and product name '{productName}'.");

        return _transactionService.Get(orderNumber, productName);
    }
}