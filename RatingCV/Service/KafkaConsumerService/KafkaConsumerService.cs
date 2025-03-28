using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RatingCV.Data;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;

namespace RatingCV.Service.KafkaConsumerService;

public class KafkaConsumerService : IKafkaConsumerService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConsumerConfig _consumerConfig;
    private readonly IKafkaProcessingService _processingService;

    public KafkaConsumerService(
        ILogger<KafkaConsumerService> logger,
        IServiceScopeFactory scopeFactory,
        IKafkaProcessingService processingService)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _processingService = processingService;

        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            GroupId = "cv-data-group",
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = false,
            EnableAutoOffsetStore = false,
            AllowAutoCreateTopics = true,
            EnablePartitionEof = true,
            MaxPollIntervalMs = 300000,
            SessionTimeoutMs = 60000,
            HeartbeatIntervalMs = 3000
        };
    }
    public async Task StartConsumingAsync(List<string> topics, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe(topics);
        _logger.LogInformation("✅ Kafka Consumer started, listening on topics: {Topics}", string.Join(", ", topics));

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    if (consumeResult == null)
                    {
                        _logger.LogWarning("⚠️ Received null message from Kafka");
                        continue;
                    }

                    _logger.LogInformation("📥 Received message from {Topic}: {Message}", consumeResult.Topic, consumeResult.Value);

                    // Gọi dịch vụ xử lý dữ liệu
                    await _processingService.ProcessMessageAsync(consumeResult.Topic, consumeResult.Value, cancellationToken);

                    consumer.Commit(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError("❌ Kafka consume error on {Topic}: {Error}", topics, ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError("❌ Unexpected error in Kafka Consumer for {Topic}: {Error}", topics, ex.Message);
                }
            }
        }
        finally
        {
            consumer.Close();
            _logger.LogInformation("🛑 Kafka Consumer stopped for topics: {Topics}", string.Join(", ", topics));
        }
    }
}
