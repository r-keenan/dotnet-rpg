using System.Text.Json.Serialization;

namespace dotnet_rpg.Models
{
    //This will change the Schema for this enum from indexes to strings in Swagger
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum RpgClass
    {
        Knight,
        Mage,
        Cleric
    }
}