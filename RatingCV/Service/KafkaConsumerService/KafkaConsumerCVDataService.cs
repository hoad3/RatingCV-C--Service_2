using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RatingCV.Data;
using RatingCV.Helpers;
using RatingCV.Model.cv_ungvien;
using RatingCV.Service.FileService;
namespace RatingCV.Service.KafkaConsumerService;

public class KafkaConsumerCVDataService : BackgroundService
{
    private readonly IKafkaConsumerService _kafkaConsumerService;
    private readonly List<string> _topics = new() { "cv-data" }; // Danh sách topic

    public KafkaConsumerCVDataService(IKafkaConsumerService kafkaConsumerService)
    {
        _kafkaConsumerService = kafkaConsumerService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tasks = _topics.Select(topic => _kafkaConsumerService.StartConsumingAsync(topic, stoppingToken));
        return Task.WhenAll(tasks);
    }
}

