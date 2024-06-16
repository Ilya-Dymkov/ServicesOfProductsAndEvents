using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Transaction
{
    public static Transaction CreateInstance(Order order, Product product, uint quantity, uint discount) =>
        new()
        {
            Guid = Guid.NewGuid(),
            Order = order,
            Product = product,
            Price = product.Price * (100 - discount) / 100,
            Quantity = quantity
        };

    [JsonIgnore]
    public Guid Guid { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
    public uint Price { get; set; }
    public uint Quantity { get; set; }
}