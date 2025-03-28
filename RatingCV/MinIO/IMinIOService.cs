namespace RatingCV.MinIO;

public interface IMinIOService
{
    Task<string> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType);
    Task<bool> DeleteFileAsync(string bucketName, string objectName);
    Task<string> GetFileUrl(string bucketName, string objectName);
    

}