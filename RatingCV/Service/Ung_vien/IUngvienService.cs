using Microsoft.AspNetCore.Mvc;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;

namespace RatingCV.Service.Ung_vien;

public interface IUngvienService
{
    Task<List<CvUngVienDto>> ListUngvien();
    Task<List<thong_tin_chi_tiet_ungvien>> GetInfoUngvien();
    Task<List<cv_ungvien>> FindUngvienWithTech(List<string> keywords);
    Task<rating_cv> AddRatingCV([FromBody] rating_cv_Dto ratingCvDto, Stream fileStream, string fileName, string contentType);
  
}