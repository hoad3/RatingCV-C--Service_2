using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RatingCV.Model.du_an;

namespace RatingCV.Model.cv_ungvien;

public class cv_ungvien
{
    public int ungvienid { get; set; } // Primary Key (auto-increment)
    
    [JsonPropertyName("name")]
    public string ten_ung_vien { get; set; }

    [JsonPropertyName("phone")]
    public string sdt { get; set; }

    [JsonPropertyName("email")]
    public string email { get; set; }
    
    [JsonPropertyName("address")]
    public string dia_chi { get; set; }
        
    [JsonPropertyName("ten_cv")]
    public string ten_cv { get; set; }
    
    [JsonPropertyName("github")]
    public string github { get; set; }
    
    [NotMapped]
    [JsonPropertyName("projects")]
    public List<Project>? projects { get; set; }
    

}