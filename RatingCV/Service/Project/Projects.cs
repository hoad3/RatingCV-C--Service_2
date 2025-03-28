using Microsoft.EntityFrameworkCore;
using RatingCV.Data;
using RatingCV.Model.du_an;

namespace RatingCV.Service.Project;

public class Projects: IProjects
{
    private readonly AppDbContext _context;

    public Projects(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<du_an>> GetProjectsAsync(int userId)
    {
        var projects = await _context.du_an
            .Where(d => d.userid == userId)
            .ToListAsync();
        return projects;
    }
}