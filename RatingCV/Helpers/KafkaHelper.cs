using Confluent.Kafka;

namespace RatingCV.Helpers;

public static class KafkaHelper
{
    public static ConsumerConfig GetConsumerConfig(ILogger logger)
    {
        var bootstrapServers = "localhost:29092";
        var groupId = "cv-data-group";
            
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = false,
            EnableAutoOffsetStore = false,
            MaxPollIntervalMs = 300000,
            SessionTimeoutMs = 60000,
            HeartbeatIntervalMs = 3000,
            AllowAutoCreateTopics = true,
            EnablePartitionEof = true
        };

        logger.LogInformation($"Kafka configuration: BootstrapServers={config.BootstrapServers}, GroupId={config.GroupId}");
        return config;
    }
}