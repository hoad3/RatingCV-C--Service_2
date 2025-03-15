using System.Text.Json;
using RatingCV.Data;
using RatingCV.Model.cv_ungvien;

namespace RatingCV.Service.KafkaConsumerService;

public class KafkaProcessingService:IKafkaProcessingService
{
    private readonly ILogger<KafkaProcessingService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaProcessingService(ILogger<KafkaProcessingService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task ProcessMessageAsync(string message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            var cvData = JsonSerializer.Deserialize<cv_ungvien>(message);
            if (cvData == null)
            {
                _logger.LogWarning("⚠️ Deserialized message is null");
                return;
            }

            await dbContext.cv_ungvien.AddAsync(cvData, cancellationToken);
            var changes = await dbContext.SaveChangesAsync(cancellationToken);

            if (changes > 0)
            {
                _logger.LogInformation("✅ Successfully saved candidate data to database");
            }
            else
            {
                _logger.LogWarning("⚠️ No changes detected in database!");
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError("❌ JSON deserialization error: {Error} - Raw Data: {Message}", ex.Message, message);
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Error processing message: {Error}", ex.Message);
        }
    }
}