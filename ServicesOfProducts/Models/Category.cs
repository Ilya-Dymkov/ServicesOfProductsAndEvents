using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Category
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
}