using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Product
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public Category Category { get; set; }
    public uint Cost { get; set; }
}