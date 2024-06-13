using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Order
{
    public Guid Guid { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; }
}