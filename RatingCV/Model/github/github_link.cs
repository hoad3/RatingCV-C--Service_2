using System.Text.Json.Serialization;
using RatingCV.Model.cv_ungvien;

namespace RatingCV.Model.github;

public class github_link
{
    public int id { get; set; }
    
    public int userid { get; set; }
    
    public string github { get; set; }
    
    public cv_ungvien.cv_ungvien cv_ungvien { get; set; }
}