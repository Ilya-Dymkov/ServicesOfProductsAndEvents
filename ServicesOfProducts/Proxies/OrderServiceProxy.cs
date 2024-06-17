using System.Text;
using ServicesOfProducts.DataContext;
using ServicesOfProducts.Loggers;
using ServicesOfProducts.Loggers.LoggersSource;
using ServicesOfProducts.Models;
using ServicesOfProducts.Models.ModelsSource;
using ServicesOfProducts.Services;
using ServicesOfProducts.Services.ServicesSource;

namespace ServicesOfProducts.Proxies;

public class OrderServiceProxy(ApplicationDbContext dbContext) : IOrderService
{
    private readonly IOrderService _orderService = new OrderService(dbContext);
    private readonly IProxyLogger _logger = ProxyLogger.CreateInstance<IOrderService>();
    
    public Task<IEnumerable<Order>> GetAll()
    {
        _logger.ToLogInfo("Get all orders.");

        return _orderService.GetAll();
    }

    public Task<Order?> Get(uint number)
    {
        _logger.ToLogInfo($"Get order with number '{number}'.");

        return _orderService.Get(number);
    }

    public Task<IEnumerable<Transaction>> GetTransactions(uint orderNumber)
    {
        _logger.ToLogInfo($"Get transactions with order number '{orderNumber}'.");

        return _orderService.GetTransactions(orderNumber);
    }

    public Task<Order?> Add(uint number, string userLogin, InputProductInfo product)
    {
        _logger.ToLogInfo($"Add new order with number '{number}', user login '{userLogin}' and product with " +
                          $"name '{product.Name}', quantity '{product.Quantity}', discount '{product.Discount}'.");

        return _orderService.Add(number, userLogin, product);
    }

    public Task<Order?> AddMany(uint number, string userLogin, IEnumerable<InputProductInfo> products)
    {
        var sb = new StringBuilder($"Add new order with number '{number}', user login '{userLogin}' and products:\n");
        foreach (var product in products) sb.Append(
            $"\tproduct name '{product.Name}', quantity '{product.Quantity}', discount '{product.Discount}';\n");
        
        _logger.ToLogInfo(sb.ToString());

        return _orderService.AddMany(number, userLogin, products);
    }

    public Task SoftDelete(uint number)
    {
        _logger.ToLogInfo($"Soft delete order with number '{number}'.");

        return _orderService.SoftDelete(number);
    }

    public Task HardDelete(uint number)
    {
        _logger.ToLogInfo($"Hard delete order with number '{number}'.");

        return _orderService.HardDelete(number);
    }
}