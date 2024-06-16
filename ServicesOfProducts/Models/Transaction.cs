using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Transaction
{
    public static Transaction CreateInstance(Order order, Product product) =>
        new()
        {
            Guid = Guid.NewGuid(),
            Order = order,
            Product = product,
            Cost = product.Cost
        };

    [JsonIgnore]
    public Guid Guid { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
    public uint Cost { get; set; }
}