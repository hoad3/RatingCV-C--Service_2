using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using RatingCV.Data;

namespace RatingCV.MinIO;

public class MinIOService:IMinIOService
{
    private readonly IMinioClient _minioClient;
    private readonly AppDbContext _context;
    private static Dictionary<string, Timer> _fileTimers = new Dictionary<string, Timer>();

    public MinIOService(IMinioClient minioClient, AppDbContext context)
    {
        _context = context;
        _minioClient = minioClient;
    }

    private async Task EnsureBucketExistsAsync(string bucketName)
    {
        bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
        }
    }

    public async Task<string> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType)
    {
        try
        {
            await EnsureBucketExistsAsync(bucketName);

            var res = await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(data.Length)
                .WithContentType(contentType));
            Console.WriteLine("File uploaded successfully!");
            return res.ObjectName;
        }
        catch (MinioException ex)
        {
            Console.WriteLine($"Lỗi khi upload file: {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string bucketName, string objectName)
    {
        try
        {
            var statObject = await _minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName));

            if (statObject != null)
            {
                await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName));

                Console.WriteLine($"File {objectName} đã bị xóa thành công.");
                return true;
            }
        }
        catch (MinioException ex)
        {
            Console.WriteLine($"Lỗi trong quá trình xóa file: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi không xác định: {ex.Message}");
            return false;
        }

        return false;
    }

    public Task<string> GetFileUrl(string bucketName, string objectName)
    {
        return _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(60 * 60 * 6));
    }
}