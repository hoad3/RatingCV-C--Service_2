using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RatingCV.Data;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;

namespace RatingCV.Service.KafkaConsumerService;

public class KafkaProcessingService : IKafkaProcessingService
{
    private readonly ILogger<KafkaProcessingService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaProcessingService(ILogger<KafkaProcessingService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task ProcessMessageAsync(string topic, string message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            if (topic == "cv-data")
            {
                await HandleCvData(message, dbContext);
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

    private async Task HandleCvData(string message, AppDbContext dbContext)
    {
        var cvData = JsonSerializer.Deserialize<cv_ungvien>(message);
        if (cvData == null || string.IsNullOrEmpty(cvData.sdt))
        {
            _logger.LogWarning("⚠️ Dữ liệu CV không hợp lệ");
            return;
        }

        dbContext.cv_ungvien.Add(cvData);
        await dbContext.SaveChangesAsync();
        _logger.LogInformation($"✅ Lưu ứng viên ID: {cvData.ungvienid}, phone: {cvData.sdt}");

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
    }

    private async Task HandleInfoUngvienAsync(string message, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var infoData = JsonSerializer.Deserialize<thong_tin_chi_tiet_ungvien>(message);
        if (infoData == null || string.IsNullOrEmpty(infoData.phone))
        {
            _logger.LogWarning("⚠️ Dữ liệu info không hợp lệ");
            return;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 🔍 Tìm ứng viên theo số điện thoại trước khi lưu
            var existingCandidate = await dbContext.cv_ungvien
                .FirstOrDefaultAsync(c => c.sdt == infoData.phone, cancellationToken);

            if (existingCandidate != null)
            {
                infoData.ungvienid = existingCandidate.ungvienid;  // ✅ Gán luôn ungvienid
                _logger.LogInformation($"📌 Tìm thấy ứng viên: {infoData.phone}, gán ungvienid: {infoData.ungvienid}");
            }
            else
            {
                infoData.ungvienid = 0; // Nếu chưa có, để 0 và cập nhật sau
                _logger.LogWarning($"⚠️ Chưa tìm thấy ứng viên cho phone: {infoData.phone}. Sẽ cập nhật sau.");
            }

            // ✅ Lưu dữ liệu với ungvienid đã có (hoặc 0 nếu chưa tìm thấy)
            dbContext.thong_tin_chi_tiet_ungvien.Add(infoData);
            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"✅ Lưu thông tin ứng viên với ID tạm: {infoData.id}, ungvienid: {infoData.ungvienid}");

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError("❌ Lỗi xử lý info-ungvien: {Error}", ex.Message);
        }
    }
}
