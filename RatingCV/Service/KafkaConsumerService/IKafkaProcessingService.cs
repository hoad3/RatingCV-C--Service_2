namespace RatingCV.Service.KafkaConsumerService;

public interface IKafkaProcessingService
{
    Task ProcessMessageAsync(string message, CancellationToken cancellationToken);
}