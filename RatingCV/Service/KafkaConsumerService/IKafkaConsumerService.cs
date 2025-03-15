using RatingCV.Data;

namespace RatingCV.Service.KafkaConsumerService;

public interface IKafkaConsumerService
{
    Task StartConsumingAsync(List<string> topics, CancellationToken cancellationToken);
    
}