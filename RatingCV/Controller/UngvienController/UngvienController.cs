using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RatingCV.Model.cv_ungvien;
using RatingCV.Model.session;
using RatingCV.Model.Thong_tin_chi_tiet_ungvien;
using RatingCV.Service.Session;
using RatingCV.Service.Ung_vien;
using Renci.SshNet;

namespace RatingCV.Controller.UngvienController;

[ApiController]
[Route("api/[controller]")]
public class UngvienController : ControllerBase
{
    private readonly IUngvienService _ungvienService;
    private readonly ISessionService _sessionService;

    public UngvienController(IUngvienService ungvienService, ISessionService sessionService)
    {
        _sessionService = sessionService;
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

    [HttpPost]
    [Route("AddSession")]
    public async Task<IActionResult> AddSession([FromBody] sessionDto session)
    {
        var result = await _sessionService.AddSession(session);

        return Ok(result);
    }

    [HttpDelete]
    [Route("RemoveSession/{sessionId}")]
    public async Task<IActionResult> RemoveSession(int sessionId)
    {
        var results = await _sessionService.DeleteSession(sessionId);
        return Ok(results);
    }

    [HttpGet]
    [Route("GetSession")]
    public async Task<IActionResult> GetSession()
    {
        var session = await _sessionService.GetSession();
        if (session == null)
        {
            return BadRequest();
        }
        return Ok(session);
    }

    [HttpPost("AddRatingCV")]
    public async Task<IActionResult> AddRatingCV([FromForm] rating_cv_Dto ratingCvDto, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is required.");
        }
        using var fileStream = file.OpenReadStream();
        var fileName = file.FileName;
        var contentType = file.ContentType;
        try
        {
            var result = await _ungvienService.AddRatingCV(ratingCvDto, fileStream, fileName, contentType);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    

    // [HttpPost("download-github")]
    // public async Task<ActionResult<string>> DownloadGithubProject([FromQuery] int userId, [FromQuery] string githubUrl)
    // {
    //     var result = await _ungvienService.DownloadAndSaveGithubProject(userId, githubUrl);
    //     return Ok(result);
    // }
    
}