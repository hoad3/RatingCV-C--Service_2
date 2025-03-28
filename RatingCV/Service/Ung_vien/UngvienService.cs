using System.Diagnostics;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using RatingCV.Data;
using RatingCV.MinIO;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Octokit;

namespace RatingCV.Service.Ung_vien;

public class UngvienService: IUngvienService
{
    private readonly AppDbContext _context;
    private readonly IMinIOService _minio;
    private readonly HttpClient _httpClient;
    private readonly IGitHubClient _githubClient;

    public UngvienService(AppDbContext context, IMinIOService minio, HttpClient httpClient)
    {
        _minio = minio;
        _context = context;
        _httpClient = httpClient;
        _githubClient = new GitHubClient(new ProductHeaderValue("RatingCV"));
    }
    public async Task<List<CvUngVienDto>> ListUngvien()
    {
       var ungvien = await _context.cv_ungvien.ToListAsync();
       
       var ungvienDtos = new List<CvUngVienDto>();

       foreach (var uv in ungvien)
       {    
           var fileurl = !string.IsNullOrEmpty(uv.ten_cv)
               ? await _minio.GetFileUrl("ratingcv", uv.ten_cv)
               : null;

           ungvienDtos.Add(new CvUngVienDto
           {
               ungvienid = uv.ungvienid,
               name = uv.ten_ung_vien,
               phone = uv.sdt,
               email = uv.email,
               dia_chi = uv.dia_chi,
               ten_cv = fileurl,
               github = uv.github,
           });
       }

       return ungvienDtos;
    }

    public async Task<List<thong_tin_chi_tiet_ungvien>> GetInfoUngvien()
    {
        var ungvien = await _context.thong_tin_chi_tiet_ungvien.ToListAsync();

        var ungviens = ungvien.Select(u => new thong_tin_chi_tiet_ungvien
        {
            id = u.id,
            ungvienid = u.ungvienid,
            hoc_van = u.hoc_van,
            chung_chi = u.chung_chi,
            cong_nghe = u.cong_nghe,
            framework = u.framework,
            data_base = u.data_base,
            kinh_nghiem = u.kinh_nghiem,
            phone = u.phone
        }).ToList();
        
        return ungviens;
    }

    public async Task<List<cv_ungvien>> FindUngvienWithTech(List<string> keywords)
    {
        var lowerKeywords = keywords
            .Select(k => k.Replace(" ", "").Trim().ToLower())
            .ToList(); 
        var ungvienIds = await _context.thong_tin_chi_tiet_ungvien
            .Where(u => 
                u.cong_nghe.Any(tech => lowerKeywords.Any(k => tech.Replace(" ", "").Trim().ToLower().Contains(k))) ||  
                u.framework.Any(fw => lowerKeywords.Any(k => fw.Replace(" ", "").Trim().ToLower().Contains(k))) ||     
                u.data_base.Any(db => lowerKeywords.Any(k => db.Replace(" ", "").Trim().ToLower().Contains(k))))        
            .Select(u => u.ungvienid)
            .Distinct()
            .ToListAsync();
        
        if (!ungvienIds.Any())
            return new List<cv_ungvien>();
        
        var matchungvien = await _context.cv_ungvien
            .Where(cv => ungvienIds.Contains(cv.ungvienid))
            .ToListAsync();

        return matchungvien;
    }
    
}