﻿using ServicesOfProducts.Models;
using ServicesOfProducts.Models.ModelsSource;

namespace ServicesOfProducts.Services.ServicesSource;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAll();
    Task<Order?> Get(uint number);
    Task<IEnumerable<Transaction>> GetTransactions(uint orderNumber);
    Task<Order?> Add(uint number, string userLogin, InputProductInfo product);
    Task<Order?> AddMany(uint number, string userLogin, IEnumerable<InputProductInfo> products);
    Task SoftDelete(uint number);
    Task HardDelete(uint number);
}