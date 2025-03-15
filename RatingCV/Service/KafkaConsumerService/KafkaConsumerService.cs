using Confluent.Kafka;

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

    public async Task StartConsumingAsync(string topic, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe(topic);
        _logger.LogInformation("✅ Kafka Consumer started, listening on topic: {Topic}", topic);

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

                    _logger.LogInformation("📥 Received message from {Topic}: {Message}", topic, consumeResult.Value);

                    // Gọi dịch vụ xử lý dữ liệu
                    await _processingService.ProcessMessageAsync(consumeResult.Value, cancellationToken);

                    consumer.Commit(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError("❌ Kafka consume error on {Topic}: {Error}", topic, ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError("❌ Unexpected error in Kafka Consumer for {Topic}: {Error}", topic, ex.Message);
                }
            }
        }
        finally
        {
            consumer.Close();
            _logger.LogInformation("🛑 Kafka Consumer stopped for topic: {Topic}", topic);
        }
    }
}
