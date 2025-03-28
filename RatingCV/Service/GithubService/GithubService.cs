using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RatingCV.Data;
using RatingCV.MinIO;
using RatingCV.Model;
using RatingCV.Service.FileService;

namespace RatingCV.Service.GithubService;

public class GithubService : IGithubService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly IMinIOService _minIOService;

    public GithubService(AppDbContext context, HttpClient httpClient, IMinIOService minIOService)
    {
        _context = context;
        _httpClient = httpClient;
        _minIOService = minIOService;
    }
    
    

} 