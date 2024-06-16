using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Category
{
    public static Category CreateInstance(string name, Category? parentCategory) =>
        new()
        {
            Guid = Guid.NewGuid(),
            Name = name,
            ParentCategory = parentCategory
        };

    [JsonIgnore]
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public Category? ParentCategory { get; set; }
}