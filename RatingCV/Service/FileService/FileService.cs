using System.Text.Json;
using Confluent.Kafka;
using RatingCV.DTO.FileNameDTO;
using RatingCV.MinIO;

namespace RatingCV.Service.FileService;

public class FileService: BackgroundService
{
     private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<FileService> _logger;

    public FileService(IServiceScopeFactory serviceScopeFactory, ILogger<FileService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            GroupId = "file-data-group",
            AutoOffsetReset = AutoOffsetReset.Latest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("cv-file");

        _logger.LogInformation("FileService is listening for messages on Kafka topic: cv-file");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    _logger.LogInformation($"Received file data from Kafka.");

                    // Giải mã message từ JSON
                    var fileMessage = JsonSerializer.Deserialize<FileMessageDto>(consumeResult.Value);
                    if (fileMessage != null && !string.IsNullOrEmpty(fileMessage.FileContent))
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var minioService = scope.ServiceProvider.GetRequiredService<IMinIOService>();

                        // Giải mã Base64 và lưu lên MinIO
                        string fileUrl = await DecodeAndUploadFileAsync(minioService, fileMessage.FileContent, fileMessage.FileName);
                        _logger.LogInformation($"File uploaded successfully: {fileUrl}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing file-data message: {ex.Message}");
                }
            }
        }
        finally
        {
            consumer.Close();
        }
    }

    public async Task<string> DecodeAndUploadFileAsync(IMinIOService minioService, string base64String, string originalFileName)
    {
        try
        {
            if (base64String.Contains(","))
            {
                base64String = base64String.Split(',')[1]; 
            }

            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(base64String);
            }
            catch (FormatException ex)
            {
                _logger.LogError($"Invalid Base64 format: {ex.Message}");
                throw;
            }

            // Xác định loại file dựa trên phần mở rộng
            string contentType = GetMimeType(originalFileName);
            
            using var stream = new MemoryStream(fileBytes);

            string fileUrl = await minioService.UploadFileAsync(originalFileName, stream, contentType);
            _logger.LogInformation($"Uploaded file {originalFileName} to MinIO at {fileUrl}");
            return fileUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error decoding and uploading file: {ex.Message}");
            throw;
        }
    }

    private string GetMimeType(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();
        return extension switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }
}