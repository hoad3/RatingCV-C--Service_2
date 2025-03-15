using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using RatingCV.Data;

namespace RatingCV.MinIO;

public class MinIOService:IMinIOService
{
     // private readonly IMinioClient _minioClient;
    private readonly IMinioClient _minioClient;
    private readonly AppDbContext _context;
    private static Dictionary<string, Timer> _fileTimers = new Dictionary<string, Timer>();
    public string BucketName => "ratingcv";

    public MinIOService(IMinioClient minioClient, AppDbContext context)
    {
        _context = context;
        _minioClient = minioClient;
        
     
    }

    // Phương thức upload file lên MinIO
    public async Task<string> UploadFileAsync(string objectName, Stream data, string contentType)
    {
        try
        {
            // Kiểm tra xem bucket có tồn tại không
            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(BucketName));
            if (!found)
            {
                // Nếu bucket chưa tồn tại, tạo mới bucket
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName));
            }

            // Upload file lên MinIO
            var res = await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
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
    
    public async Task<bool> DeleteFileAsync(string objectName)
    {
        try
        {
            // Kiểm tra xem file có tồn tại trong bucket không
            var statObject= await _minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName));

            if (statObject != null)
            {
                // Thực hiện xóa file khỏi MinIO
                await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectName));

                Console.WriteLine($"File {objectName} đã bị xóa thành công.");
                return true;
            }
        }
        catch (MinioException ex)
        {
            // Xử lý các lỗi từ MinIO (như bucket không tồn tại hoặc object không tìm thấy)
            Console.WriteLine($"Lỗi trong quá trình xóa file: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Xử lý các lỗi chung
            Console.WriteLine($"Lỗi không xác định: {ex.Message}");
            return false;
        }

        return false;
    }
    
    
    // Phương thức lấy URL của file từ MinIO
    public Task<string> GetFileUrl(string objectName)
    {
        return _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(BucketName)
            .WithObject(objectName)
            .WithExpiry(60 * 60 * 6));
    }
    
}