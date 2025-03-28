using Microsoft.AspNetCore.Mvc;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;
using RatingCV.Service.Ung_vien;

namespace RatingCV.Controller.UngvienController;

[ApiController]
[Route("api/[controller]")]
public class UngvienController : ControllerBase
{
    private readonly IUngvienService _ungvienService;

    public UngvienController(IUngvienService ungvienService)
    {
        _ungvienService = ungvienService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CvUngVienDto>>> GetUngvien()
    {
        var ungvien = await _ungvienService.ListUngvien();
        return Ok(ungvien);
    }

    [HttpGet("info")]
    public async Task<ActionResult<List<thong_tin_chi_tiet_ungvien>>> GetInfoUngvien()
    {
        var ungvien = await _ungvienService.GetInfoUngvien();
        return Ok(ungvien);
    }

    [HttpPost]
    [Route("GetUngvienByCongNghe")]
    public async Task<IActionResult> GetUngvienByCongNghe([FromBody] List<string> technologies)
    {
        if (technologies == null || !technologies.Any())
            return BadRequest("Danh sách công nghệ không được để trống.");

        var result = await _ungvienService.FindUngvienWithTech(technologies);

        if (result == null || !result.Any())
            return NotFound("Không tìm thấy ứng viên phù hợp.");
        return Ok(result);
    }
    

    // [HttpPost("download-github")]
    // public async Task<ActionResult<string>> DownloadGithubProject([FromQuery] int userId, [FromQuery] string githubUrl)
    // {
    //     var result = await _ungvienService.DownloadAndSaveGithubProject(userId, githubUrl);
    //     return Ok(result);
    // }
    
}