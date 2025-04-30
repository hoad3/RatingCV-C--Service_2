using System.Threading.Tasks;
using RatingCV.Model.github;

namespace RatingCV.Service.GithubService;

public interface IGithubService
{
    Task<List<github_link>> getGithubLinks(int userid);
} 