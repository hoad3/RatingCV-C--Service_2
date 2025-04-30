using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RatingCV.Model.du_an;

public class Project
{
    [NotMapped]
    [JsonPropertyName("ten_du_an")]
    public string ten_du_an { get; set; }

    [NotMapped]
    [JsonPropertyName("mo_ta")]
    public string mo_ta { get; set; }

    [NotMapped]
    [JsonPropertyName("ngay_bat_dau")]
    public string ngay_bat_dau { get; set; }

    [NotMapped]
    [JsonPropertyName("ngay_ket_thuc")]
    public string ngay_ket_thuc { get; set; }

    [NotMapped]
    [JsonPropertyName("team_size")]
    [JsonConverter(typeof(TeamSizeConverter))]
    public string team_size { get; set; }

    [NotMapped]
    [JsonPropertyName("role")]
    public string role { get; set; }
    
}

public class TeamSizeConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return reader.GetInt32().ToString();
            case JsonTokenType.String:
                return reader.GetString();
            default:
                return "1"; // Default value if neither number nor string
        }
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}