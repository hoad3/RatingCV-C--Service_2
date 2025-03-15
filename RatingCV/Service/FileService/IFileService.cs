using RatingCV.MinIO;

namespace RatingCV.Service.FileService;

public interface IFileService
{

    Task<string> DecodeAndUploadFileAsync(IMinIOService minioService, string base64String, string originalFileName);
}