using System.Text.Json.Serialization;

namespace RatingCV.Model.cv_ungvien;

public class CvUngVienDto
{
    [JsonPropertyName("name")]
    public string name { get; set; }

    [JsonPropertyName("phone")]
    public string phone { get; set; }

    [JsonPropertyName("email")]
    public string email { get; set; }
    
    [JsonPropertyName("dia_chi")]
    public string dia_chi { get; set; }

    [JsonPropertyName("github")]
    public string github { get; set; }

    [JsonPropertyName("ten_cv")]
    public string ten_cv { get; set; }
}