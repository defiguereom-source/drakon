using System;
using System.Collections.Generic;
using System.Text;
using Drakon.Application.DTOs;
using Drakon.Application.Interfaces;
using Drakon.Domain;
using Drakon.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Drakon.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IAppDbContext _db;
    public ProjectService(IAppDbContext db) => _db = db;

    public async Task<List<ProjectDto>> GetMyProjectsAsync(int userId)
        => await _db.Projects
            .Where(p => p.Members.Any(m => m.UserId == userId))
            .Select(p => new ProjectDto(p.Id, p.Name, p.Key, p.Description, p.OwnerId, p.Tickets.Count))
            .ToListAsync();

    public async Task<ProjectDto> CreateAsync(int userId, CreateProjectDto dto)
    {
        var key = dto.Key.Trim().ToUpperInvariant();
        if (await _db.Projects.AnyAsync(p => p.Key == key))
            throw new InvalidOperationException("Ya existe un proyecto con esa clave.");

        var project = new Project
        {
            Name = dto.Name.Trim(),
            Key = key,
            Description = dto.Description,
            OwnerId = userId
        };
        project.Members.Add(new ProjectMember { UserId = userId, Role = UserRole.Admin });

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        return new ProjectDto(project.Id, project.Name, project.Key, project.Description, userId, 0);
    }

    public async Task AddMemberAsync(int userId, int projectId, AddMemberDto dto)
    {
        var isAdmin = await _db.ProjectMembers.AnyAsync(m =>
            m.ProjectId == projectId && m.UserId == userId && m.Role == UserRole.Admin);
        if (!isAdmin)
            throw new UnauthorizedAccessException("Solo un admin puede agregar miembros.");

        if (!await _db.Users.AnyAsync(u => u.Id == dto.UserId))
            throw new KeyNotFoundException("Usuario no encontrado.");

        if (await _db.ProjectMembers.AnyAsync(m => m.ProjectId == projectId && m.UserId == dto.UserId))
            throw new InvalidOperationException("El usuario ya es miembro.");

        _db.ProjectMembers.Add(new ProjectMember
        {
            ProjectId = projectId,
            UserId = dto.UserId,
            Role = dto.Role
        });
        await _db.SaveChangesAsync();
    }

    public async Task EnsureMemberAsync(int userId, int projectId)
    {
        if (!await _db.ProjectMembers.AnyAsync(m => m.ProjectId == projectId && m.UserId == userId))
            throw new UnauthorizedAccessException("No perteneces a este proyecto.");
    }
}