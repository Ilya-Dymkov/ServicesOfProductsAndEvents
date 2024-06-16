using System.Text.Json.Serialization;

namespace ServicesOfProducts.Models.ModelsSource;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Genders
{
    Male,
    Female,
    Helicopter,
    Unknown
}