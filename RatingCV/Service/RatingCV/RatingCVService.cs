using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RatingCV.Data;
using RatingCV.MinIO;
using RatingCV.Model;

namespace RatingCV.Service.RatingCV;

public class RatingCVService: IRatingCVService
{
    private readonly AppDbContext _context;
    private readonly IMinIOService _minIOService;

    public RatingCVService(AppDbContext context, IMinIOService minIOService)
    {
        _context = context;
        _minIOService = minIOService;
    }
    
    public async Task<List<object>> GetAllRatingCVWithFileContent(int session_id)
    {
        var ratingCVs = _context.rating_cv.Where(r => r.session_id == session_id);

        var result = new List<object>();
        using var httpClient = new HttpClient();

        foreach (var ratingCV in ratingCVs)
        {
            if (!string.IsNullOrEmpty(ratingCV.file_json))
            {
                try
                {
                    var fileUrl = await _minIOService.GetFileUrl("ratingcv", ratingCV.file_json);
                    var fileContent = await httpClient.GetStringAsync(fileUrl);

                    var parsedContent = JsonSerializer.Deserialize<object>(fileContent);

                    result.Add(new
                    {
                        RatingCV = ratingCV,
                        FileContent = parsedContent
                    });
                }
                catch (Exception ex)
                {
                    result.Add(new
                    {
                        RatingCV = ratingCV,
                        FileContent = $"Error reading file: {ex.Message}"
                    });
                }
            }
            else
            {
                result.Add(new
                {
                    RatingCV = ratingCV,
                    FileContent = "No file available"
                });
            }
        }

        return result;
    }
    
    public async Task<danh_gia_theo_tieu_chi> danh_gia_theo_tieu_chi([FromBody] danh_gia_theo_tieu_chi_DTO ratingCvDto, 
        Stream fileStream, 
        string fileName, 
        string contentType)
    {
        var ratingCv = await _context.rating_cv
            .FirstOrDefaultAsync(rc => rc.session_id == ratingCvDto.id_session);

        if (ratingCv == null)
        {
            throw new InvalidOperationException($"No rating_cv found with session_id {ratingCvDto.id_session}.");
        }

        var bucketName = "ratingcv"; 
        var uploadedFileName = await _minIOService.UploadFileAsync(bucketName, fileName, fileStream, contentType);
    
        var ratingcv = new danh_gia_theo_tieu_chi()
        {
            id_rating = ratingCv.id,       
            id_session = ratingCvDto.id_session,
            ten_file = uploadedFileName,
            id_ungvien = ratingCvDto.id_ungvien,
        };
    
        _context.danh_gia_theo_tieu_chi.Add(ratingcv);
        await _context.SaveChangesAsync();

        return ratingcv;
    }
    
    public async Task<List<object>> Get_danh_gia_theo_tieu_chi(int session_id)
    {
        var ratingCVs = _context.danh_gia_theo_tieu_chi.Where(r => r.id_session == session_id);

        var result = new List<object>();
        using var httpClient = new HttpClient();

        foreach (var ratingCV in ratingCVs)
        {
            if (!string.IsNullOrEmpty(ratingCV.ten_file))
            {
                try
                {
                    var fileUrl = await _minIOService.GetFileUrl("ratingcv", ratingCV.ten_file);
                    var fileContent = await httpClient.GetStringAsync(fileUrl);

                    var parsedContent = JsonSerializer.Deserialize<object>(fileContent);

                    result.Add(new
                    {
                        RatingCV = ratingCV,
                        FileContent = parsedContent
                    });
                }
                catch (Exception ex)
                {
                    result.Add(new
                    {
                        RatingCV = ratingCV,
                        FileContent = $"Error reading file: {ex.Message}"
                    });
                }
            }
            else
            {
                result.Add(new
                {
                    RatingCV = ratingCV,
                    FileContent = "No file available"
                });
            }
        }

        return result;
    }
    
    public async Task<bool> UpdateFileAsync(int id_rating, int id_session, int id_ungvien, IFormFile newFile)
    {
        var danhGia = await _context.danh_gia_theo_tieu_chi
            .FirstOrDefaultAsync(x =>
                x.id_danh_gia == id_rating &&
                x.id_session == id_session &&
                x.id_ungvien == id_ungvien);

        if (danhGia == null)
        {
            Console.WriteLine("Không tìm thấy đánh giá phù hợp.");
            return false;
        }

        var fileName = danhGia.ten_file;

        var deleted = await _minIOService.DeleteFileAsync("ratingcv", fileName);
        if (!deleted)
        {
            Console.WriteLine("Không thể xóa file cũ.");
            return false;
        }

        using (var stream = newFile.OpenReadStream())
        {
            var result = await _minIOService.UploadFileAsync("ratingcv", fileName, stream, newFile.ContentType);
            if (result == null)
            {
                Console.WriteLine("Tải file mới lên thất bại.");
                return false;
            }
        }

        Console.WriteLine("Cập nhật file thành công.");
        return true;
    }
}