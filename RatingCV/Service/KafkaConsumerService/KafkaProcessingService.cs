using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RatingCV.Data;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.du_an;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;
using RatingCV.Service.GithubService;
using Microsoft.Extensions.Logging;
using RatingCV.Model.github;

namespace RatingCV.Service.KafkaConsumerService;

public class KafkaProcessingService : IKafkaProcessingService
{
    private readonly ILogger<KafkaProcessingService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaProcessingService(
        ILogger<KafkaProcessingService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task ProcessMessageAsync(string topic, string message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var githubService = scope.ServiceProvider.GetRequiredService<IGithubService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            if (topic == "cv-data")
            {
                await HandleCvData(message, dbContext, githubService);
            }
            else if (topic == "info-ungvien")
            {
                await HandleInfoUngvienAsync(message, dbContext, cancellationToken);
            }
            else
            {
                _logger.LogWarning("⚠️ Nhận topic không xác định: {Topic}", topic);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Lỗi xử lý message từ Kafka ({Topic}): {Error}", topic, ex.Message);
        }
    }

    private async Task HandleCvData(string message, AppDbContext dbContext, IGithubService githubService)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            var cvData = JsonSerializer.Deserialize<cv_ungvien>(message, options);
            if (cvData == null)
            {
                _logger.LogError("❌ Không thể parse dữ liệu CV từ message");
                return;
            }

            _logger.LogInformation($"📝 Đang xử lý dữ liệu CV cho ứng viên: {cvData.ten_ung_vien}");

            var existingUngvien = await dbContext.cv_ungvien.FindAsync(cvData.ungvienid);
            if (existingUngvien != null)
            {
                _logger.LogInformation($"🔄 Cập nhật thông tin ứng viên ID: {cvData.ungvienid}");
                dbContext.Entry(existingUngvien).CurrentValues.SetValues(cvData);
                dbContext.cv_ungvien.Update(existingUngvien);
            }
            else
            {
                _logger.LogInformation($"➕ Thêm mới ứng viên ID: {cvData.ungvienid}");
                await dbContext.cv_ungvien.AddAsync(cvData);
            }

            await dbContext.SaveChangesAsync();
            _logger.LogInformation($"✅ Đã lưu thông tin ứng viên ID: {cvData.ungvienid}");
            var jsonDoc = JsonDocument.Parse(message);
            if (jsonDoc.RootElement.TryGetProperty("github_link", out var githubLinksProperty))
            {
                var githubLinks = githubLinksProperty.EnumerateArray()
                    .Select(x => x.GetString())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                if (githubLinks.Any())
                {
                    _logger.LogInformation($"🔗 Đang xử lý {githubLinks.Count} GitHub links");
                    
                    // Xóa các GitHub links cũ của ứng viên
                    var existingLinks = await dbContext.github_link
                        .Where(g => g.userid == cvData.ungvienid)
                        .ToListAsync();
                    
                    if (existingLinks.Any())
                    {
                        _logger.LogInformation($"🗑️ Xóa {existingLinks.Count} GitHub links cũ");
                        dbContext.github_link.RemoveRange(existingLinks);
                    }

                    // Thêm các GitHub links mới
                    foreach (var link in githubLinks)
                    {
                        var githubLink = new github_link
                        {
                            userid = cvData.ungvienid,
                            github = link
                        };
                        await dbContext.github_link.AddAsync(githubLink);
                        _logger.LogInformation($"➕ Thêm GitHub link: {link}");
                    }

                    await dbContext.SaveChangesAsync();
                    _logger.LogInformation($"✅ Đã lưu thành công {githubLinks.Count} GitHub links cho ứng viên ID: {cvData.ungvienid}");
                }
                else
                {
                    _logger.LogWarning("⚠️ Không có GitHub links nào để lưu");
                }
            }
            else
            {
                _logger.LogWarning("⚠️ Không tìm thấy trường github_link trong message");
            }

            // Cập nhật ungvienid trong bảng thong_tin_chi_tiet_ungvien nếu có bản ghi chờ
            var pendingRecords = await dbContext.thong_tin_chi_tiet_ungvien
                .Where(t => t.phone == cvData.sdt && t.ungvienid == 0)
                .ToListAsync();

            if (pendingRecords.Any())
            {
                foreach (var record in pendingRecords)
                {
                    record.ungvienid = cvData.ungvienid;
                }
                dbContext.thong_tin_chi_tiet_ungvien.UpdateRange(pendingRecords);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation($"🔄 Cập nhật ungvienid {cvData.ungvienid} cho {pendingRecords.Count} bản ghi đang chờ.");
            }
            
            if (cvData.projects != null && cvData.projects.Count > 0)
            {
                var duAnList = cvData.projects.Select(p => new du_an()
                {
                    userid = cvData.ungvienid,
                    ten_du_an = p.ten_du_an,
                    mo_ta = p.mo_ta,
                    ngay_bat_dau = p.ngay_bat_dau,
                    ngay_ket_thuc = p.ngay_ket_thuc,
                    team_size = p.team_size?.ToString() ?? "1", // Chuyển đổi sang string
                    role = p.role,
                }).ToList();

                dbContext.du_an.AddRange(duAnList);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation($"✅ Lưu {duAnList.Count} dự án cho ứng viên ID: {cvData.ungvienid}");
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError($"❌ Lỗi parse JSON: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Lỗi xử lý dữ liệu CV: {ex.Message}");
            throw;
        }
    }

    private async Task HandleInfoUngvienAsync(string message, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var infoData = JsonSerializer.Deserialize<thong_tin_chi_tiet_ungvien>(message, options);
            if (infoData == null || string.IsNullOrEmpty(infoData.phone))
            {
                _logger.LogWarning("⚠️ Dữ liệu info không hợp lệ");
                return;
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Tìm ứng viên theo số điện thoại
                var existingCandidate = await dbContext.cv_ungvien
                    .FirstOrDefaultAsync(c => c.sdt == infoData.phone, cancellationToken);

                if (existingCandidate != null)
                {
                    infoData.ungvienid = existingCandidate.ungvienid;
                    _logger.LogInformation($"📌 Tìm thấy ứng viên: {infoData.phone}, gán ungvienid: {infoData.ungvienid}");
                }
                else
                {
                    _logger.LogWarning($"⚠️ Chưa tìm thấy ứng viên cho phone: {infoData.phone}. Sẽ cập nhật sau.");
                    return; // Không lưu nếu chưa có ứng viên
                }

                var existingInfo = await dbContext.thong_tin_chi_tiet_ungvien
                    .FirstOrDefaultAsync(t => t.ungvienid == infoData.ungvienid, cancellationToken);

                if (existingInfo != null)
                {
                    existingInfo.hoc_van = infoData.hoc_van;
                    existingInfo.chung_chi = infoData.chung_chi;
                    existingInfo.cong_nghe = infoData.cong_nghe;
                    existingInfo.framework = infoData.framework;
                    existingInfo.data_base = infoData.data_base;
                    existingInfo.kinh_nghiem = infoData.kinh_nghiem;
                    existingInfo.phone = infoData.phone;
                    dbContext.thong_tin_chi_tiet_ungvien.Update(existingInfo);
                }
                else
                {
                    // Thêm thông tin mới
                    dbContext.thong_tin_chi_tiet_ungvien.Add(infoData);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"✅ Lưu thông tin ứng viên với ID: {infoData.id}, ungvienid: {infoData.ungvienid}");

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError("❌ Lỗi xử lý info-ungvien: {Error}", ex.Message);
                throw;
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError($"❌ Lỗi parse JSON: {ex.Message}");
            throw;
        }
    }
}
