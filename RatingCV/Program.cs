using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using RatingCV.Data;
using RatingCV.MinIO;
using RatingCV.Service;
using RatingCV.Service.FileService;
using RatingCV.Service.KafkaConsumerService;
using RatingCV.Service.Project;
using RatingCV.Service.Ung_vien;
using RatingCV.SSH;


var builder = WebApplication.CreateBuilder(args);


Env.Load();
// Lấy giá trị từ biến môi trường
string sshHost = Environment.GetEnvironmentVariable("SSH_HOST") ?? "localhost";
string sshUser = Environment.GetEnvironmentVariable("SSH_USER") ?? "";
string sshPassword = Environment.GetEnvironmentVariable("SSH_PASSWORD") ?? "";
string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "";
string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "";
string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
string dbSslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable";


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sshTunnel = new SshTunnelManager(
    sshHost: sshHost,
    sshUser: sshUser,
    sshPassword: sshPassword,
    dbHost: dbHost,
    dbPort: 5432,
    localPort: 5433
);

sshTunnel.StartTunnel();

// 🔹 Cấu hình kết nối đến PostgreSQL qua SSH Tunnel
string connectionString = $"Host=127.0.0.1;Port={sshTunnel.LocalPort};Database={dbName};Username={dbUser};Password={dbPassword};SSL Mode={dbSslMode};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Đọc cấu hình MinIO từ biến môi trường
var minioEndpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT") ?? "";
var minioAccessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY") ?? "";
var minioSecretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY") ?? "";

builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<MinIOSetting>>().Value;

    return new MinioClient()
        .WithEndpoint(minioEndpoint)
        .WithCredentials(minioAccessKey, minioSecretKey)
        .WithSSL()
        .Build();
});

builder.Services.AddScoped<IProjects, Projects>();
builder.Services.AddScoped<IUngvienService, UngvienService>();

builder.Services.AddScoped<IMinIOService, MinIOService>();



// Đăng ký Kafka Consumer là một BackgroundService
builder.Services.AddHostedService<KafkaConsumerCVDataService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<IKafkaProcessingService, KafkaProcessingService>();
builder.Services.AddHostedService<FileService>();
builder.Services.AddHttpClient();
// builder.Services.AddHostedService<FileService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseRouting();
app.MapControllers();


app.MapGet("/", () => "Kafka Consumer Service is running!");


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader()
        .AllowAnyOrigin();
    options.AllowAnyMethod();
    options.WithOrigins();
});

app.Run();

