using System.Text.Json.Serialization;

namespace RatingCV.Model.Thong_tin_chi_tiet_ungvien;

public class thong_tin_chi_tiet_ungvien
{
    public int id { get; set; }
    public int ungvienid { get; set; }
    
    [JsonPropertyName("hoc_van")]
    public string hoc_van { get; set; }
    
    [JsonPropertyName("chung_chi")]
    public string chung_chi { get; set; }
    
    [JsonPropertyName("cong_nghe")]
    public List<string> cong_nghe { get; set; }
    
    [JsonPropertyName("framework")]
    public List<string> framework { get; set; }
    
    [JsonPropertyName("data_base")]
    public List<string> data_base { get; set; }
    
    [JsonPropertyName("kinh_nghiem")]
    public string kinh_nghiem { get; set; }
    
    [JsonPropertyName("phone")]
    public string phone { get; set; }
}