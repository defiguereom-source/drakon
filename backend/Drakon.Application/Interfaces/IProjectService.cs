using Drakon.Application.DTOs;

namespace Drakon.Application.Interfaces;

public interface IProjectService
{
    Task<List<ProjectDto>> GetMyProjectsAsync(int userId);
    Task<ProjectDto> CreateAsync(int userId, CreateProjectDto dto);
    Task AddMemberAsync(int userId, int projectId, AddMemberDto dto);
    Task EnsureMemberAsync(int userId, int projectId);
}