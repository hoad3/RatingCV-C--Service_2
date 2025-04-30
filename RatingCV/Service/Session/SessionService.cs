using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RatingCV.Data;
using RatingCV.MinIO;
using RatingCV.Model.session;

namespace RatingCV.Service.Session;

public class SessionService: ISessionService
{
    private readonly AppDbContext _context;
    private readonly IMinIOService _minIOService;

    public SessionService(AppDbContext context, IMinIOService minIOService)
    {
        _context = context;
        _minIOService = minIOService;
    }

    public async Task<session> AddSession([FromBody] sessionDto sessionDto)
    {
        var session = new session()
        {
            session_name = sessionDto.session_name
        };
        
        _context.session.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<session> DeleteSession(int sessionId)
    {
        var findSession = await _context.session.FirstOrDefaultAsync(s => s.session_id == sessionId);
    
        if (findSession == null)
        {
            throw new InvalidOperationException($"Session with ID {sessionId} not found.");
        }

        var relatedRatings = await _context.rating_cv
            .Where(r => r.session_id == sessionId)
            .ToListAsync();

        if (relatedRatings.Any())
        {
            foreach (var rating in relatedRatings)
            {
                if (!string.IsNullOrEmpty(rating.file_json))
                {
                    try
                    {
                        await _minIOService.DeleteFileAsync("ratingcv", rating.file_json);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file {rating.file_json} from MinIO: {ex.Message}");
                    }
                }
            }

            _context.rating_cv.RemoveRange(relatedRatings);
        }

        _context.session.Remove(findSession);
    
        await _context.SaveChangesAsync();
    
        return findSession;
    }

    public async Task<List<session>> GetSession()
    {
        return await _context.session.ToListAsync();
    }
}