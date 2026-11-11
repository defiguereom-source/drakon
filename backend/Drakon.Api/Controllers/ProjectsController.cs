using Drakon.Api.Extensions;
using Drakon.Application.DTOs;
using Drakon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Drakon.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projects;
    public ProjectsController(IProjectService projects) => _projects = projects;

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetMine()
        => Ok(await _projects.GetMyProjectsAsync(User.GetUserId()));

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        => Ok(await _projects.CreateAsync(User.GetUserId(), dto));

    [HttpPost("{projectId:int}/members")]
    public async Task<IActionResult> AddMember(int projectId, AddMemberDto dto)
    {
        await _projects.AddMemberAsync(User.GetUserId(), projectId, dto);
        return NoContent();
    }
}