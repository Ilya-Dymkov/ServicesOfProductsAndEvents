using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class Order
{
    public static Order CreateInstance(uint number, User user) =>
        new()
        {
            Guid = Guid.NewGuid(),
            Number = number,
            User = user,
            DateOrder = DateTime.Now
        };

    [JsonIgnore]
    public Guid Guid { get; set; }
    public uint Number { get; set; }
    public User User { get; set; }
    public DateTime DateOrder { get; set; }
    public bool IsDeleted { get; set; }
}