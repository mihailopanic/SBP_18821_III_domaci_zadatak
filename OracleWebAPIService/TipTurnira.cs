using System.Text.Json.Serialization;

namespace OracleWebAPIService;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipTurnira
{
    TakmicarskiNormalni,
    TakmicarskiBrzopotezni,
    EgzibicioniNormalni,
    EgzibicioniBrzopotezni
}
