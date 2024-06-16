using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ServicesOfProducts.Models.ModelsSource;

namespace ServicesOfProducts.Models;

[PrimaryKey(nameof(Guid))]
public class User
{
    public static User CreateInstance(string login, string name, Genders gender, DateTime birthday, bool isAdmin) =>
        new()
        {
            Guid = Guid.NewGuid(),
            Login = login,
            Name = name,
            Gender = gender,
            Birthday = birthday,
            IsAdmin = isAdmin
        };

    [JsonIgnore]
    public Guid Guid { get; set; }
    public string Login { get; set; }
    public string Name { get; set; }
    public Genders Gender { get; set; }
    public DateTime Birthday { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsDeleted { get; set; }
}