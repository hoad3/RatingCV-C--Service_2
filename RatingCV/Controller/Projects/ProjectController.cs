using Microsoft.AspNetCore.Mvc;
using RatingCV.Service.GithubService;
using RatingCV.Service.Project;

namespace RatingCV.Controller.Projects;

public class ProjectController: ControllerBase
{
    private readonly IProjects _projects;
    private readonly IGithubService _githubService;

    public ProjectController(IProjects projects, IGithubService githubService)
    {
        _githubService = githubService;
        _projects = projects;
    }

    [HttpGet]
    [Route("/projects/{userid}")]
    public async Task<IActionResult> GetProjects(int userid)
    {
        var projects = await _projects.GetProjectsAsync(userid);

        if (projects == null)
        {
            return NotFound();
        }
        
        return Ok(projects);
    }

    [HttpGet]
    [Route("github_link/{userid}")]
    public async Task<IActionResult> getGithubLink(int userid)
    {
        var githubs = await _githubService.getGithubLinks(userid);

        if (githubs == null)
        {
            return NotFound();
        }
        
        return Ok(githubs);
    }
    
}