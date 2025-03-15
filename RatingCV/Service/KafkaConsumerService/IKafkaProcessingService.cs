namespace RatingCV.Service.KafkaConsumerService;

public interface IKafkaProcessingService
{
    Task ProcessMessageAsync(string topic, string message, CancellationToken cancellationToken);
    
}