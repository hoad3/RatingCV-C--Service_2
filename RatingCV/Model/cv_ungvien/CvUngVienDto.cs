using System.Text.Json.Serialization;

namespace RatingCV.Model.cv_ungvien;

public class CvUngVienDto
{
    public int ungvienid { get; set; }
  
    public string name { get; set; }

  
    public string phone { get; set; }

  
    public string email { get; set; }
    
    
    public string dia_chi { get; set; }

   
    public string github { get; set; }

   
    public string ten_cv { get; set; }
}