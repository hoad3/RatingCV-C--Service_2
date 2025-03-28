namespace RatingCV.Model.Thong_tin_chi_tiet_ungvien;

public class UngvieninfoDtos
{
    public int id { get; set; }
    public int ungvienid { get; set; }
    
    public string hoc_van { get; set; }
    
    public string chung_chi { get; set; }
    
    public List<string> cong_nghe ;
    public List<string> framework ;
    public List<string> data_base ;
    
    public string kinh_nghiem { get; set; }
    public string phone { get; set; }
    

}