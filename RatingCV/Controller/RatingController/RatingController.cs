using Microsoft.AspNetCore.Mvc;
using RatingCV.Model;
using RatingCV.Service.RatingCV;

namespace RatingCV.Controller.RatingController;

public class RatingController: ControllerBase
{
    private readonly IRatingCVService _ratingCVService;

    public RatingController(IRatingCVService ratingCVService)
    {
        _ratingCVService = ratingCVService;
    }

    [HttpGet]
    [Route("/rating_Info/{session_id}")]
    public async Task<ActionResult> GetRatingInfo(int session_id)
    {
        var results = await _ratingCVService.GetAllRatingCVWithFileContent(session_id);

        if (results == null)
        {
            return BadRequest();
        }
        
        return Ok(results);
    }
    
    [HttpPost]
    [Route("/add_rating_cadidate")]
    public async Task<ActionResult> GetRatingInfo([FromForm] danh_gia_theo_tieu_chi_DTO ratingCvDto, IFormFile file)
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
            var result = await _ratingCVService.danh_gia_theo_tieu_chi(ratingCvDto, fileStream, fileName, contentType);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpGet]
    [Route("/cadidate_rating_Info/{session_id}")]
    public async Task<ActionResult> Get_danh_gia_theo_tieu_chi(int session_id)
    {
        var results = await _ratingCVService.Get_danh_gia_theo_tieu_chi(session_id);

        if (results == null)
        {
            return BadRequest();
        }
        
        return Ok(results);
    }
    [HttpPost]
    [Route("update-file")]
    public async Task<IActionResult> UpdateRatingCVFile(
        [FromForm] int id_rating,
        [FromForm] int id_session,
        [FromForm] int id_ungvien,
        [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File không hợp lệ.");

        // using var stream = file.OpenReadStream();
        var result = await _ratingCVService.UpdateFileAsync(id_rating, id_session, id_ungvien, file);

        if (!result)
            return StatusCode(500, "Cập nhật file thất bại.");

        return Ok("Cập nhật file thành công.");
    }
    
}