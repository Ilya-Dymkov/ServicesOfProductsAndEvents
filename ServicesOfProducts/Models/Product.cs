using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Product
{
    public static Product CreateInstance(string name, Category? category, uint price, uint quantity, bool enable) =>
        new()
        {
            Guid = Guid.NewGuid(),
            Name = name,
            Category = category,
            Price = price,
            Quantity = quantity,
            Enable = enable
        };

    [JsonIgnore]
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public Category? Category { get; set; }
    public uint Price { get; set; }
    public uint Quantity { get; set; }
    public bool Enable { get; set; }
}