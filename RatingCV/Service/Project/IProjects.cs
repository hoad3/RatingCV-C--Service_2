using RatingCV.Model.du_an;

namespace RatingCV.Service.Project;

public interface IProjects
{
    Task<List<du_an>> GetProjectsAsync(int userId);
}