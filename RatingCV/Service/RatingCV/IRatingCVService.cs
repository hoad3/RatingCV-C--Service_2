using Microsoft.AspNetCore.Mvc;
using RatingCV.Model;

namespace RatingCV.Service.RatingCV;

public interface IRatingCVService
{
    Task<List<object>> GetAllRatingCVWithFileContent(int session_id);
    Task<danh_gia_theo_tieu_chi> danh_gia_theo_tieu_chi([FromBody] danh_gia_theo_tieu_chi_DTO ratingCvDto, Stream fileStream, string fileName, string contentType);
    Task<List<object>> Get_danh_gia_theo_tieu_chi(int session_id);
    Task<bool> UpdateFileAsync(int id_rating, int id_session, int id_ungvien, IFormFile newFile);
}