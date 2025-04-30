using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RatingCV.Data;
using RatingCV.MinIO;
using RatingCV.Model;
using RatingCV.Model.github;
using RatingCV.Service.FileService;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace RatingCV.Service.GithubService;

public class GithubService : IGithubService
{
    private readonly AppDbContext _context;


    public GithubService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<github_link>> getGithubLinks(int userid)
    {
        var githubLinks = await _context.github_link
            .Where(g => g.userid == userid)
            .ToListAsync();
        return githubLinks;
    }

    // public async Task ProcessGithubLinksFromKafka(string jsonData, int userId)
    // {
    //     try
    //     {
    //         _logger.LogInformation($"📝 Bắt đầu xử lý GitHub links cho ứng viên ID: {userId}");
    //         
    //         // Parse JSON data to get array of GitHub links
    //         var jsonDoc = JsonDocument.Parse(jsonData);
    //         
    //         // Kiểm tra xem có tồn tại ứng viên không
    //         var ungvien = await _context.cv_ungvien.FindAsync(userId);
    //         if (ungvien == null)
    //         {
    //             _logger.LogWarning($"⚠️ Không tìm thấy ứng viên với ID: {userId}");
    //             return;
    //         }
    //
    //         // Lấy danh sách GitHub links từ message
    //         if (!jsonDoc.RootElement.TryGetProperty("github_link", out var githubLinksProperty))
    //         {
    //             _logger.LogWarning("⚠️ Không tìm thấy trường github_link trong message");
    //             return;
    //         }
    //
    //         var githubLinks = githubLinksProperty.EnumerateArray()
    //             .Select(x => x.GetString())
    //             .Where(x => !string.IsNullOrEmpty(x))
    //             .ToList();
    //
    //         if (!githubLinks.Any())
    //         {
    //             _logger.LogWarning("⚠️ Không có GitHub links nào để lưu");
    //             return;
    //         }
    //
    //         _logger.LogInformation($"📝 Tìm thấy {githubLinks.Count} GitHub links");
    //
    //         // Remove existing GitHub links for this user
    //         var existingLinks = await _context.github_link
    //             .Where(g => g.userid == userId)
    //             .ToListAsync();
    //
    //         if (existingLinks.Any())
    //         {
    //             _logger.LogInformation($"🗑️ Xóa {existingLinks.Count} GitHub links cũ");
    //             _context.github_link.RemoveRange(existingLinks);
    //         }
    //
    //         // Add new GitHub links
    //         foreach (var link in githubLinks)
    //         {
    //             var githubLink = new github_link
    //             {
    //                 userid = userId,
    //                 github = link
    //             };
    //             await _context.github_link.AddAsync(githubLink);
    //             _logger.LogInformation($"➕ Thêm GitHub link: {link}");
    //         }
    //
    //         await _context.SaveChangesAsync();
    //         _logger.LogInformation($"✅ Đã lưu thành công {githubLinks.Count} GitHub links cho ứng viên ID: {userId}");
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "❌ Lỗi xử lý GitHub links cho ứng viên ID: {UserId}", userId);
    //         throw;
    //     }
    // }
    // public async Task ProcessGithubLinksMessage(string message)
    // {
    //     try
    //     {
    //         var options = new JsonSerializerOptions
    //         {
    //             PropertyNameCaseInsensitive = true
    //         };
    //
    //         var jsonDoc = JsonDocument.Parse(message);
    //         
    //         // Kiểm tra xem message có chứa github_link không
    //         if (jsonDoc.RootElement.TryGetProperty("github_link", out var githubLinksProperty))
    //         {
    //             // Lấy ungvienid từ message
    //             if (jsonDoc.RootElement.TryGetProperty("ungvienid", out var userIdProperty))
    //             {
    //                 var userId = userIdProperty.GetInt32();
    //                 _logger.LogInformation($"📝 Đang xử lý GitHub links cho ứng viên ID: {userId}");
    //                 await ProcessGithubLinksFromKafka(message, userId);
    //             }
    //             else
    //             {
    //                 _logger.LogWarning("⚠️ Message không chứa ungvienid");
    //             }
    //         }
    //         else
    //         {
    //             _logger.LogInformation("ℹ️ Message không chứa github_link");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "❌ Lỗi xử lý message: {Message}", message);
    //         throw;
    //     }
    // }
} 