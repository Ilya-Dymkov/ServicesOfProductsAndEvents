using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Product
{
    public static Product CreateInstance(string name, Category? category, uint cost, uint count, bool enable) =>
        new()
        {
            Guid = Guid.NewGuid(),
            Name = name,
            Category = category,
            Cost = cost,
            Count = count,
            Enable = enable
        };

    [JsonIgnore]
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public Category? Category { get; set; }
    public uint Cost { get; set; }
    public uint Count { get; set; }
    public bool Enable { get; set; }
}