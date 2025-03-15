namespace RatingCV.Service.KafkaConsumerService;

public interface IKafkaConsumerService
{
    Task StartConsumingAsync(string topic, CancellationToken cancellationToken);
}