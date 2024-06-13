using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Transaction
{
    public Guid Guid { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
    public uint Cost { get; set; }
    public DateTime Date { get; set; }
}