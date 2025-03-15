namespace RatingCV.MinIO;

public interface IMinIOService
{
    Task<string> UploadFileAsync(string objectName, Stream data, string contentType);
    Task<bool> DeleteFileAsync(string objectName);
    Task<string> GetFileUrl(string objectName);
    

}