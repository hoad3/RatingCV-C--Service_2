using Microsoft.AspNetCore.Mvc;
using RatingCV.Service.Project;

namespace RatingCV.Controller.Projects;

public class ProjectController: ControllerBase
{
    private readonly IProjects _projects;

    public ProjectController(IProjects projects)
    {
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
    
}