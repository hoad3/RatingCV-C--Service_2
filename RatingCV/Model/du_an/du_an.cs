using System.Text.Json.Serialization;

namespace RatingCV.Model.du_an;

public class du_an
{
    
    public int du_an_id { get; set; }
    
    public int userid { get; set; }
    
    [JsonPropertyName("ten_du_an")]
    public string ten_du_an { get; set; }
    
    [JsonPropertyName("mo_ta")]
    public string mo_ta  { get; set; }
    
    [JsonPropertyName("ngay_bat_dau")]
    public string ngay_bat_dau { get; set; }
    
    [JsonPropertyName("ngay_ket_thuc")]
    public string ngay_ket_thuc { get; set; }
    
    [JsonPropertyName("team_size")]
    public string team_size  { get; set; }
    
    [JsonPropertyName("role")]
    public string role { get; set; }
    
    [JsonPropertyName("github")]
    public string github { get; set; }
    
}