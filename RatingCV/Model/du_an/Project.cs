using System.ComponentModel.DataAnnotations.Schema;
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
    public string team_size { get; set; }

    [NotMapped]
    [JsonPropertyName("role")]
    public string role { get; set; }

    [NotMapped]
    [JsonPropertyName("github")]
    public string github { get; set; }
}